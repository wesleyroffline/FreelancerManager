const API_BASE_URL = 'http://localhost:5148/api';

// Variáveis Globais
const clientForm = document.getElementById('client-form');
const contractForm = document.getElementById('contract-form');
const receiptForm = document.getElementById('receipt-form');
const messageDiv = document.getElementById('message');

const selectClient = document.getElementById('contractClientId');
const selectContract = document.getElementById('receiptContractId');

// ==========================================
// LÓGICA DE ABAS (TABS)
// ==========================================
const tabButtons = document.querySelectorAll('.tab-btn');
const tabContents = document.querySelectorAll('.tab-content');

tabButtons.forEach(btn => {
    btn.addEventListener('click', () => {
        tabButtons.forEach(b => b.classList.remove('active'));
        tabContents.forEach(c => c.style.display = 'none');

        btn.classList.add('active');
        const targetId = btn.getAttribute('data-target');
        const targetContent = document.getElementById(targetId);
        targetContent.style.display = 'contents';
    });
});

// ==========================================
// UTILITÁRIOS
// ==========================================
function showMessage(text, type) {
    messageDiv.textContent = text;
    messageDiv.className = `message ${type}`;
    messageDiv.classList.remove('hidden');
    setTimeout(() => messageDiv.classList.add('hidden'), 4000);
}

function formatCurrency(value) {
    return Number(value || 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

// ==========================================
// CLIENTES
// ==========================================
clientForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = {
        name: document.getElementById('clientName').value,
        email: document.getElementById('clientEmail').value,
        document: document.getElementById('clientDocument').value
    };

    try {
        const res = await fetch(`${API_BASE_URL}/clients`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        if (!res.ok) throw new Error('Erro ao cadastrar cliente.');

        showMessage('Cliente cadastrado!', 'success');
        clientForm.reset();
        loadClients();
    } catch (err) { showMessage(err.message, 'error'); }
});

async function loadClients() {
    try {
        const res = await fetch(`${API_BASE_URL}/clients`);
        const clients = await res.json();

        // 1. Preenche Tabela de Consulta (Atualizado com a coluna Ações)
        const tbody = document.getElementById('clients-table-body');
        tbody.innerHTML = clients.length === 0 ? '<tr><td colspan="4" class="empty-state">Nenhum cliente.</td></tr>' : '';
        clients.forEach(c => {
            tbody.innerHTML += `
                <tr>
                    <td><strong>${c.name}</strong></td>
                    <td>${c.email}</td>
                    <td>${c.document}</td>
                    <td><button class="btn-delete" onclick="deleteClient('${c.id}')">Excluir</button></td>
                </tr>`;
        });

        // 2. Preenche Dropdown de Contratos
        selectClient.innerHTML = '<option value="">Selecione um cliente...</option>';
        clients.forEach(c => {
            selectClient.innerHTML += `<option value="${c.id}">${c.name}</option>`;
        });
    } catch (err) { console.error(err); }
}

// NOVA FUNÇÃO: Excluir Cliente com validação de contrato
async function deleteClient(id) {
    try {
        // 1. Busca todos os contratos para ver se este cliente tem algum
        const resContracts = await fetch(`${API_BASE_URL}/contracts`);
        const contracts = await resContracts.json();

        // 2. Verifica se existe algum contrato com o ID deste cliente
        const hasContract = contracts.some(c => c.clientId === id);

        // 3. Se tiver contrato, avisa e bloqueia
        if (hasContract) {
            showMessage("Cliente possui um contrato no sistema. Não é possível excluir.", "error");
            return;
        }

        // 4. Se não tiver, pede confirmação e exclui
        if (!confirm("Tem certeza que deseja excluir este cliente?")) return;

        const res = await fetch(`${API_BASE_URL}/clients/${id}`, { method: 'DELETE' });
        if (!res.ok) throw new Error('Erro ao excluir cliente.');

        showMessage('Cliente excluído com sucesso!', 'success');
        loadClients();
    } catch (err) { showMessage(err.message, 'error'); }
}

// ==========================================
// CONTRATOS
// ==========================================
contractForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = {
        clientId: selectClient.value,
        title: document.getElementById('contractTitle').value,
        totalValue: parseFloat(document.getElementById('contractTotalValue').value),
        startDate: new Date(document.getElementById('contractStartDate').value).toISOString()
    };

    try {
        const res = await fetch(`${API_BASE_URL}/contracts`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        if (!res.ok) throw new Error('Erro ao criar contrato.');

        showMessage('Contrato criado!', 'success');
        contractForm.reset();
        loadContracts();
    } catch (err) { showMessage(err.message, 'error'); }
});

async function loadContracts() {
    try {
        const res = await fetch(`${API_BASE_URL}/contracts`);
        const contracts = await res.json();

        const tbody = document.getElementById('contracts-table-body');
        tbody.innerHTML = contracts.length === 0 ? '<tr><td colspan="4" class="empty-state">Nenhum contrato.</td></tr>' : '';
        contracts.forEach(c => {
            const date = new Date(c.startDate).toLocaleDateString('pt-BR');
            const statusBadge = c.status === 1 ? '<span style="color:#16a34a; font-weight:bold;">Ativo</span>' : 'Outro';

            tbody.innerHTML += `
                <tr>
                    <td><strong>${c.title}</strong></td>
                    <td>${formatCurrency(c.totalValue)}</td>
                    <td>${date}</td>
                    <td>${statusBadge}</td>
                </tr>`;
        });

        selectContract.innerHTML = '<option value="">Selecione um contrato...</option>';
        contracts.forEach(c => {
            selectContract.innerHTML += `<option value="${c.id}">${c.title} (${formatCurrency(c.totalValue)})</option>`;
        });
    } catch (err) { console.error(err); }
}

// ==========================================
// RECIBOS
// ==========================================
receiptForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = {
        contractId: selectContract.value,
        receiptNumber: document.getElementById('receiptNumber').value,
        grossValue: parseFloat(document.getElementById('grossValue').value),
        customTaxPercentage: parseFloat(document.getElementById('customTaxPercentage').value),
        paymentMethod: document.getElementById('paymentMethod').value
    };

    try {
        const res = await fetch(`${API_BASE_URL}/receipts`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        if (!res.ok) {
            let errorMessage = 'Erro ao emitir recibo.';
            try {
                const errorData = await res.json();
                errorMessage = errorData.error || errorData.message || errorMessage;
            } catch { }
            throw new Error(errorMessage);
        }

        showMessage('Recibo emitido!', 'success');
        receiptForm.reset();
        loadReceiptsTable();
    } catch (err) { showMessage(err.message, 'error'); }
});

async function loadReceiptsTable() {
    try {
        const res = await fetch(`${API_BASE_URL}/receipts`);
        const receipts = await res.json();

        const tbody = document.getElementById('receipts-table-body');
        tbody.innerHTML = receipts.length === 0 ? '<tr><td colspan="7" class="empty-state">Nenhum recibo emitido.</td></tr>' : '';

        receipts.forEach(r => {
            const date = new Date(r.issuedAt).toLocaleDateString('pt-BR');
            tbody.innerHTML += `
                <tr>
                    <td><strong>${r.receiptNumber}</strong></td>
                    <td>${formatCurrency(r.grossValue)}</td>
                    <td style="color: var(--danger); font-weight:600;">- ${formatCurrency(r.taxDeductions)}</td>
                    <td style="color: var(--success-text); font-weight:700;">${formatCurrency(r.netValue)}</td>
                    <td><span class="pill-payment">${r.paymentMethod}</span></td>
                    <td>${date}</td>
                    <td><button class="btn-delete" onclick="deleteReceipt('${r.id}')">Excluir</button></td>
                </tr>`;
        });
    } catch (err) { console.error(err); }
}

async function deleteReceipt(id) {
    if (!confirm('Excluir este recibo?')) return;
    try {
        const res = await fetch(`${API_BASE_URL}/receipts/${id}`, { method: 'DELETE' });
        if (!res.ok) throw new Error('Erro ao excluir.');
        showMessage('Recibo excluído!', 'success');
        loadReceiptsTable();
    } catch (err) { showMessage(err.message, 'error'); }
}

// ==========================================
// INICIALIZAÇÃO
// ==========================================
document.addEventListener('DOMContentLoaded', () => {
    loadClients();
    loadContracts();
    loadReceiptsTable();
});