
function ComisionViewController() {
    this.ViewName = "TablaComision";
    this.ApiEndPointName = "Comision";

    this.InitView = function () {
        console.log("Comision Init View");
        this.LoadTable();

        //Definir que el boton de create debe llamar al metodo
        $("#btnCreateComision").click(function () {
            var vc = new ComisionViewController();
            vc.Create();
        })

        $("#btnUpdateComision").click(function () {
            var vc = new ComisionViewController();
            vc.Update();
        });

        $("#btnDeleteComision").click(function () {
            var vc = new ComisionViewController();
            vc.Delete();
        });

    }

    // Cargar la tabla con datos de administradores desde la API
    this.LoadTable = function () {
        var ca = new ControlActions();
        var service = this.ApiEndPointName + "/RetrieveAll";
        var urlService = ca.GetUrlApiService(service);

        var columns = [];
        columns[0] = { 'data': 'id' };
        columns[1] = { 'data': 'nombre' };
        columns[2] = { 'data': 'tipo' };
        columns[3] = { 'data': 'created' };
        columns[4] = { 'data': 'idAdmin' };
        columns[5] = { 'data': 'porcentaje' };
        columns[6] = { 'data': 'tarifa1' };
        columns[7] = { 'data': 'tarifa2' };
        columns[8] = { 'data': 'tarifa3' };
        columns[9] = { 'data': 'montoMin' };
        columns[10] = { 'data': 'montoMax' };
        columns[11] = { 'data': 'porcentajeAsesor' };

        var table = $('#tblComisiones').DataTable({ // CORRECCIÓN: Usar #tblComisiones
            "ajax": {
                "url": urlService,
                "dataSrc": ""
            },
            columns: columns,
            destroy: true
        });

        // Asignar eventos de carga de datos en el click de la tabla (binding de data)
        $('#tblComisiones tbody').on('click', 'tr', function () { // CORRECCIÓN: Usar #tblAdmins
            var row = $(this).closest('tr');
            var comisionDTO = table.row(row).data(); // CORRECCIÓN: Obtener datos de la tabla correcta

            if (comisionDTO) {
                // Mapeo con el formulario
                $('#txtId').val(comisionDTO.id);
                $('#txtTipo').val(comisionDTO.tipo);
                $('#txtNombre').val(comisionDTO.nombre);
                $('#txtPorcentaje').val(comisionDTO.porcentaje);
                $('#txtTarifa1').val(comisionDTO.tarifa1);
                $('#txtTarifa2').val(comisionDTO.tarifa2);
                $('#txtTarifa3').val(comisionDTO.tarifa3);
                $('#txtMontoMaximo').val(comisionDTO.montoMax);
                $('#txtMontoMinimo').val(comisionDTO.montoMin);
                $('#txtPorcentajeAsesor').val(comisionDTO.porcentajeAsesor);
                $('#txtIdAdmin').val(comisionDTO.idAdmin);
            }
        });
    }

    // Método para create
    this.Create = function () {
        var comisionDTO = {};
        comisionDTO.id = 0;
        comisionDTO.tipo = $("#txtTipo").val();
        comisionDTO.nombre = $("#txtNombre").val();
        comisionDTO.porcentaje = $("#txtPorcentaje").val();
        comisionDTO.tarifa1 = $("#txtTarifa1").val();
        comisionDTO.tarifa2 = $("#txtTarifa2").val();
        comisionDTO.tarifa3 = $("#txtTarifa3").val();
        comisionDTO.montoMax = $("#txtMontoMaximo").val();
        comisionDTO.montoMin = $("#txtMontoMinimo").val();
        comisionDTO.porcentajeAsesor = $("#txtPorcentajeAsesor").val();
        comisionDTO.idAdmin = $("#txtIdAdmin").val();

        //invocar API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Create";

        ca.PostToAPI(urlService, comisionDTO, function () {
            console.log("Comision creada");

            //Recargar la tabla despues de crar el registro
            $('#tblComisiones').DataTable().ajax.reload(); // CORRECCIÓN: Usar #tblComisiones
        });
    }

    //Metodo Update
    this.Update = function () {
        var comisionId = $("#txtId").val();

        if (!comisionId) {
            Swal.fire("Error", "Debe seleccionar una comision antes de actualizar", "warning");
            return;
        }

        var comisionDTO = {
            id: comisionId,
            tipo: $("#txtTipo").val(),
            nombre: $("#txtNombre").val(),
            porcentaje: $("#txtPorcentaje").val(),
            tarifa1: $("#txtTarifa1").val(),
            tarifa2: $("#txtTarifa2").val(),
            tarifa3: $("#txtTarifa3").val(),
            montoMax: $("#txtMontoMaximo").val(),
            montoMin: $("#txtMontoMinimo").val(),
            porcentajeAsesor: $("#txtPorcentajeAsesor").val(),
            idAdmin: $("#txtIdAdmin").val()
        };

        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Update";

        Swal.fire({
            title: "¿Estás seguro?",
            text: "Los datos de la comision serán actualizados.",
            icon: "info",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, actualizar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                ca.PutToAPI(urlService, comisionDTO, function () {
                    console.log("Comision actualizadas");
                    $('#tblComisiones').DataTable().ajax.reload(); // Recargar la tabla
                });
            }
        });
    };

    // Método para eliminar usuario
    this.Delete = function () {
        var comisionId = $("#txtId").val();

        if (!comisionId) {
            Swal.fire("Error", "Debe seleccionar una comision antes de eliminar", "warning");
            return;
        }

        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Delete";

        Swal.fire({
            title: "¿Estás seguro?",
            text: "No podrás revertir esta acción",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                ca.DeleteToAPI(urlService, comisionId, function () {
                    console.log("Comision eliminada");
                    $('#tblComisiones').DataTable().ajax.reload(); // Recargar la tabla
                });
            }
        });
    };
}

// Inicialización cuando el documento esté listo
$(document).ready(function () {
    var vc = new ComisionViewController();
    vc.InitView();
});