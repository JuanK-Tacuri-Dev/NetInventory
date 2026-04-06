// ============================================================
// NetInventory — jQuery / Plugin utilities for Blazor interop
// ============================================================

// Select2 —————————————————————————————————————————————————————
window.initSelect2 = function (id, placeholder) {
    setTimeout(function () {
        var $el = $('#' + id);
        if (!$el.length) return;
        if ($el.hasClass('select2-hidden-accessible')) $el.select2('destroy');
        $el.select2({
            theme: 'bootstrap4',
            width: '100%',
            placeholder: placeholder || 'Seleccionar...',
            allowClear: true
        });
    }, 80);
};

window.getSelect2Value = function (id) {
    return $('#' + id).val() || '';
};

window.setSelect2Value = function (id, val) {
    $('#' + id).val(val).trigger('change');
};

window.destroySelect2 = function (id) {
    var $el = $('#' + id);
    if ($el.hasClass('select2-hidden-accessible')) $el.select2('destroy');
};

// DataTables ——————————————————————————————————————————————————
var dtLang = {
    search: '<i class="bi bi-search"></i> _INPUT_',
    searchPlaceholder: 'Buscar...',
    lengthMenu: 'Mostrar _MENU_ registros',
    info: 'Mostrando _START_ a _END_ de _TOTAL_ registros',
    infoEmpty: 'Sin registros',
    infoFiltered: '(filtrado de _MAX_ registros)',
    zeroRecords: 'No se encontraron resultados',
    emptyTable: 'No hay datos disponibles',
    paginate: {
        first: '«',
        previous: '‹',
        next: '›',
        last: '»'
    }
};

window.initDataTable = function (id, order) {
    setTimeout(function () {
        var $t = $('#' + id);
        if (!$t.length) return;
        if ($.fn.DataTable.isDataTable('#' + id)) {
            $('#' + id).DataTable().destroy();
        }
        $t.DataTable({
            pageLength: 15,
            lengthMenu: [10, 15, 25, 50, 100],
            order: order || [[0, 'desc']],
            language: dtLang,
            dom: "<'row'<'col-sm-6'l><'col-sm-6'f>>" +
                 "<'row'<'col-sm-12'tr>>" +
                 "<'row'<'col-sm-5'i><'col-sm-7'p>>"
        });
    }, 80);
};

window.destroyDataTable = function (id) {
    if ($.fn.DataTable.isDataTable('#' + id)) {
        $('#' + id).DataTable().destroy();
    }
};

// SweetAlert2 ————————————————————————————————————————————————
window.swalConfirm = async function (title, text, confirmText) {
    const result = await Swal.fire({
        title: title || '¿Está seguro?',
        text: text || '',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#24356a',
        cancelButtonColor: '#6c757d',
        confirmButtonText: confirmText || 'Sí, continuar',
        cancelButtonText: 'Cancelar',
        reverseButtons: true
    });
    return result.isConfirmed;
};

window.swalSuccess = function (title, text) {
    Swal.fire({ title: title, text: text, icon: 'success', confirmButtonColor: '#24356a', timer: 2000, showConfirmButton: false });
};

window.swalError = function (title, text) {
    Swal.fire({ title: title || 'Error', text: text, icon: 'error', confirmButtonColor: '#24356a' });
};

// Sidebar toggle ————————————————————————————————————————————
window.toggleSidebar = function () {
    document.body.classList.toggle('sidebar-collapsed');
};

// Loading spinner ————————————————————————————————————————————
window.showLoading = function (msg) {
    Swal.fire({ title: msg || 'Procesando...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });
};

window.hideLoading = function () {
    Swal.close();
};
