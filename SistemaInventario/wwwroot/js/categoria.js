let datatable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Bodega/ObtenerTodos"
        },
        "columns": [
            {
                "data": "nombre",
                "width": "20%"
            },
            {
                "data": "descripcion",
                "width": "40%"
            },
            {
                "data": "estado",
                "render": (data) => data == true ? "Activo" : "Inactivo",
                "width": "20%"
            },
            {
                "data": "id",
                "render": (data) => `
                    <div class="text-center">
                        <a href="/Admin/Bodega/Upsert/${data}" class="btn btn-success text-white" style="cursor: pointer"><i class="bi bi-pencil-square"></i>Editar</a>
                        <a onclick="Delete('/Admin/Bodega/Delete/${data}')" class="btn btn-danger text-white" style="cursor: pointer"><i class="bi bi-trash3-fill"></i>Eliminar</a>
                    </div>
                `,
                "width": "20%"
            }
        ]
    })
};

function Delete(url) {
    swal({
        title: "Esta seguro de Eliminar la Bodega?",
        text: "Este registro no se podrá recuperar",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((e) => {
        if (e) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                }
            })
        } else {
            toastr.error(data.message);
        }
    })
}