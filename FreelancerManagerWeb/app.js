const API_URL = 'http://localhost:5148/api/receipts';

const form = document.getElementById('receipt-form');
const tableBody = document.getElementById('receipts-table-body');
const messageDiv = document.getElementById('message');
const totalCountEl = document.getElementById('total-count');
const totalNetEl = document.getElementById('total-net');
const generateGuidBtn = document.getElementById('generate-guid-btn');
const refreshBtn = document.getElementById('refresh-btn');

function formatCurrency(value) {
    return Number(value || 0).toLocaleString('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    });
}

function updateSummary(receipts) {
    if (!totalCountEl || !totalNetEl) return;

    const count = receipts.length;
    const totalNet = receipts.reduce((acc, item) => acc + Number(item.netValue || 0), 0);

    totalCountEl.textContent = String(count);
    totalNetEl.textContent = formatCurrency(totalNet);
}

// --- 1. Listar Recibos (GET) ---
async function loadReceipts() {
    try {
        const response = await fetch(API_URL);
        if (!response.ok) throw new Error('Falha ao buscar recibos na API.');

        const receipts = await response.json();
        renderTable(receipts);
        updateSummary(receipts);
    } catch (error) {
        console.error(error);
        showMessage('Erro ao conectar com a API em http://localhost:5148. Verifique se o back-end está rodando.', 'error');
    }
}

function renderTable(receipts) {
    tableBody.innerHTML = '';

    if (!Array.isArray(receipts) || receipts.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="7" class="empty-state">Nenhum recibo encontrado.</td></tr>';
        return;
    }

    receipts.forEach(receipt => {
        const tr = document.createElement('tr');
        const date = receipt.issuedAt
            ? new Date(receipt.issuedAt).toLocaleDateString('pt-BR')
            : '-';

        tr.innerHTML = `
            <td><strong>${receipt.receiptNumber}</strong></td>
            <td>${formatCurrency(receipt.grossValue)}</td>
            <td style="color: var(--danger); font-weight: 600;">- ${formatCurrency(receipt.taxDeductions)}</td>
            <td style="color: #15803d; font-weight: 700;">${formatCurrency(receipt.netValue)}</td>
            <td><span class="pill-payment">${receipt.paymentMethod}</span></td>
            <td>${date}</td>
            <td>
                <button type="button" class="btn-delete" onclick="deleteReceipt('${receipt.id}')">Excluir</button>
            </td>
        `;
        tableBody.appendChild(tr);
    });
}

// --- 2. Criar Recibo (POST) ---
form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const receiptData = {
        contractId: document.getElementById('contractId').value.trim(),
        receiptNumber: document.getElementById('receiptNumber').value.trim(),
        grossValue: parseFloat(document.getElementById('grossValue').value),
        customTaxPercentage: parseFloat(document.getElementById('customTaxPercentage').value),
        paymentMethod: document.getElementById('paymentMethod').value
    };

    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(receiptData)
        });

        if (!response.ok) {
            let errorMessage = 'Erro ao criar recibo.';
            try {
                const errorData = await response.json();
                errorMessage = errorData.error || errorData.Error || errorData.message || errorMessage;
            } catch {
                // Mantém mensagem padrão se não houver JSON no corpo
            }
            throw new Error(errorMessage);
        }

        form.reset();
        showMessage('Recibo emitido com sucesso!', 'success');
        await loadReceipts();
    } catch (error) {
        showMessage(error.message, 'error');
    }
});

// --- 3. Excluir Recibo (DELETE) ---
async function deleteReceipt(id) {
    if (!confirm('Tem certeza que deseja excluir este recibo?')) return;

    try {
        const response = await fetch(`${API_URL}/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Erro ao excluir recibo.');

        showMessage('Recibo excluído com sucesso!', 'success');
        await loadReceipts();
    } catch (error) {
        showMessage(error.message, 'error');
    }
}

// Botão auxiliar: gerar GUID válido para testes
if (generateGuidBtn) {
    generateGuidBtn.addEventListener('click', () => {
        const guidInput = document.getElementById('contractId');
        if (guidInput) {
            guidInput.value = crypto.randomUUID();
        }
    });
}

// Botão auxiliar: atualizar lista manualmente
if (refreshBtn) {
    refreshBtn.addEventListener('click', loadReceipts);
}

let messageTimeoutId;
function showMessage(text, type) {
    clearTimeout(messageTimeoutId);
    messageDiv.textContent = text;
    messageDiv.className = `message ${type}`;

    messageTimeoutId = setTimeout(() => {
        messageDiv.classList.add('hidden');
    }, 4000);
}

document.addEventListener('DOMContentLoaded', loadReceipts);