function AsesorViewController() {

    this.ViewName = "TablaAsesor";
    this.ApiEndPointName = "Asesor";




    this.InitView = function () {
        console.log("Asesor Init View");
        this.LoadTable();

        //Definir que el boton de create debe llamar al metodo
        $("#btnCreate").click(function () {
            var vc = new AsesorViewController();
            vc.Create();
        })

        $("#btnUpdate").click(function () {
            var vc = new AsesorViewController();
            vc.Update();
        });

        $("#btnDelete").click(function () {
            var vc = new AsesorViewController();
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
        columns[1] = { 'data': 'cedula' };
        columns[2] = { 'data': 'nombre' };
        columns[3] = { 'data': 'primerApellido' };
        columns[4] = { 'data': 'segundoApellido' };
        columns[5] = { 'data': 'direccion' };
        columns[6] = { 'data': 'telefono' };
        columns[7] = { 'data': 'estado' };
        columns[8] = { 'data': 'rol' };
        columns[9] = { 'data': 'fechaNacimiento' };
        columns[10] = { 'data': 'correo' };
        columns[11] = { 'data': 'created' };
        columns[12] = {
            'data': 'ingresoComisiones',
            'render': function (data) {
                return `$${parseFloat(data).toFixed(2)}`;
            }
        }; 


        // 💡 AQUÍ definís bien la tabla y guardás la referencia
        var table = $('#tblAsesores').DataTable({
            "ajax": {
                "url": urlService,
                "dataSrc": ""
            },
            columns: columns
        });

        // Delegación más robusta
        $('#tblAsesores').on('click', 'tbody tr', function () {          
            var row = $(this).closest('tr');
            var userDTO = table.row(row).data();
            if (userDTO) {
                $('#txtCedula').val(userDTO.cedula);
                $('#txtNombre').val(userDTO.nombre);
                $('#txtPrimerApellido').val(userDTO.primerApellido);
                $('#txtSegundoApellido').val(userDTO.segundoApellido);
                $('#txtDireccion').val(userDTO.direccion);
                $('#txtTelefono').val(userDTO.telefono);
                $('#selectEstado').val(userDTO.estado);
                $('#txtPassword').val(userDTO.contrasenna);
                $('#txtEmail').val(userDTO.correo);
                $('#txtFotoPerfil').val(userDTO.fotoPerfil);
                $('#txtIngresoComisiones').val(userDTO.ingresoComisiones);

                var onlyDate = userDTO.fechaNacimiento ? userDTO.fechaNacimiento.split("T")[0] : "";
                $('#txtBirthDate').val(onlyDate);
                $('#txtId').val(userDTO.id);
            }
        });

    }

    this.Create = function () {
    var userDTO = {};
    userDTO.id = 0;
    userDTO.fechaExpiracionOTP = new Date().toISOString();
    userDTO.created = new Date().toISOString();

    userDTO.fotoPerfil = "pp";
    userDTO.cedula = $("#txtCedula").val();
    userDTO.nombre = $("#txtNombre").val();
    userDTO.primerApellido = $("#txtPrimerApellido").val();
    userDTO.segundoApellido = $("#txtSegundoApellido").val();
    userDTO.direccion = $("#txtDireccion").val();
    userDTO.telefono = $("#txtTelefono").val();
    userDTO.estado = $("#selectEstado").val();
    userDTO.rol = "Asesor"; // fijo
    userDTO.contrasenna = $("#txtPassword").val();
    userDTO.fechaNacimiento = $("#txtBirthDate").val();
    userDTO.correo = $("#txtEmail").val();

    // Exclusivo de asesor (aplicando validación de campo vacío)
    let ingreso = $("#txtIngresoComisiones").val();
    userDTO.ingresoComisiones = ingreso.trim() === "" ? 0 : parseFloat(ingreso);

    var ca = new ControlActions();
    var urlService = this.ApiEndPointName + "/Create";

    ca.PostToAPI(urlService, userDTO, function () {
        console.log("Asesor creado");
        $('#tblAsesores').DataTable().ajax.reload();
    });
}

    this.Update = function () {
        var userId = $("#txtId").val();

        if (!userId) {
            Swal.fire("Error", "Debe seleccionar un asesor antes de actualizar", "warning");
            return;
        }

        var userDTO = {
            id: userId,
            cedula: $("#txtCedula").val(),
            nombre: $("#txtNombre").val(),
            primerApellido: $("#txtPrimerApellido").val(),
            segundoApellido: $("#txtSegundoApellido").val(),
            direccion: $("#txtDireccion").val(),
            telefono: $("#txtTelefono").val(),
            estado: $("#selectEstado").val(),
            rol: "asesor",
            contrasenna: $("#txtPassword").val(),
            fechaNacimiento: $("#txtBirthDate").val(),
            correo: $("#txtEmail").val(),
            fotoPerfil: $("#txtFotoPerfil").val(),
            ingresoComisiones: parseFloat($("#txtIngresoComisiones").val())
        };

        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Update";

        Swal.fire({
            title: "¿Estás segura?",
            text: "Los datos del asesor serán actualizados.",
            icon: "info",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, actualizar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                ca.PutToAPI(urlService, userDTO, function () {
                    console.log("Asesor actualizado");
                    $('#tblAsesores').DataTable().ajax.reload();
                });
            }
        });
    };

    this.Delete = function () {
        var userId = $("#txtId").val();

        if (!userId) {
            Swal.fire("Error", "Debe seleccionar un asesor antes de eliminar", "warning");
            return;
        }

        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Delete";

        Swal.fire({
            title: "¿Estás segura?",
            text: "No podrás revertir esta acción",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                ca.DeleteToAPI(urlService, userId, function () {
                    console.log("Asesor eliminado");
                    $('#tblAsesores').DataTable().ajax.reload();
                });
            }
        });
    };





}

// $ referencia a jQuery
$(document).ready(function () {
    var vc = new AsesorViewController(); // Corrección del controlador
    vc.InitView();
});


