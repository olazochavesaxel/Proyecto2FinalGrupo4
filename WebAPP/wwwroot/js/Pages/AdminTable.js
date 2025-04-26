
function AdminViewController() {
    this.ViewName = "TablaAdmin";
    this.ApiEndPointName = "Admin";

    this.InitView = function () {
        console.log("Admin Init View");
        this.LoadTable();

        //Definir que el boton de create debe llamar al metodo
        $("#btnCreate").click(function () {
            var vc = new AdminViewController();
            vc.Create();
        })

        $("#btnUpdate").click(function () {
            var vc = new AdminViewController();
            vc.Update();
        });

        $("#btnDelete").click(function () {
            var vc = new AdminViewController();
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

        var table = $('#tblAdmins').DataTable({ // CORRECCIÓN: Usar #tblAdmins
            "ajax": {
                "url": urlService,
                "dataSrc": ""
            },
            columns: columns
        });

        // Asignar eventos de carga de datos en el click de la tabla (binding de data)
        $('#tblAdmins tbody').on('click', 'tr', function () { // CORRECCIÓN: Usar #tblAdmins
            var row = $(this).closest('tr');
            var userDTO = table.row(row).data(); // CORRECCIÓN: Obtener datos de la tabla correcta

            if (userDTO) {
                // Mapeo con el formulario
                $('#txtCedula').val(userDTO.cedula);
                $('#txtNombre').val(userDTO.nombre);
                $('#txtPrimerApellido').val(userDTO.primerApellido);
                $('#txtSegundoApellido').val(userDTO.segundoApellido);
                $('#txtDireccion').val(userDTO.direccion);
                $('#txtTelefono').val(userDTO.telefono);
                $('#selectEstado').val(userDTO.estado);
                $('#selectRol').val(userDTO.rol);
                $('#txtPassword').val(userDTO.contrasenna);
                $('#txtEmail').val(userDTO.correo);
                $('#txtFotoPerfil').val(userDTO.fotoPerfil);

                // Formato de fecha
                var onlyDate = userDTO.fechaNacimiento ? userDTO.fechaNacimiento.split("T")[0] : "";
                $('#txtBirthDate').val(onlyDate);

                $('#txtId').val(userDTO.id); // <-- AGREGAR ESTA LÍNEA
            }
        });
    }

    // Método para create
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
        userDTO.rol = $("#selectRol").val();
        userDTO.contrasenna = $("#txtPassword").val();
        userDTO.fechaNacimiento = $("#txtBirthDate").val();
        userDTO.correo = $("#txtEmail").val();

        //invocar API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Create";

        ca.PostToAPI(urlService, userDTO, function () {
            console.log("Usuario creado");

            // Guardar en localStorage
            localStorage.setItem("correoOTP", userDTO.correo);
            localStorage.setItem("origenOTP", "registro");

            //Recargar la tabla despues de crar el registro
            $('#tblAdmins').DataTable().ajax.reload(); // CORRECCIÓN: Usar #tblAdmins
        });
    }

    //Metodo Update
    this.Update = function () {
        var userId = $("#txtId").val();

        if (!userId) {
            Swal.fire("Error", "Debe seleccionar un usuario antes de actualizar", "warning");
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
            rol: $("#selectRol").val(),
            contrasenna: $("#txtPassword").val(),
            fechaNacimiento: $("#txtBirthDate").val(),
            correo: $("#txtEmail").val(),
            fotoPerfil: $("#txtFotoPerfil").val()
        };

        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Update";

        Swal.fire({
            title: "¿Estás seguro?",
            text: "Los datos del usuario serán actualizados.",
            icon: "info",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, actualizar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                ca.PutToAPI(urlService, userDTO, function () {
                    console.log("Usuario actualizado");
                    $('#tblAdmins').DataTable().ajax.reload(); // Recargar la tabla
                });
            }
        });
    };

    // Método para eliminar usuario
    this.Delete = function () {
        var userId = $("#txtId").val();

        if (!userId) {
            Swal.fire("Error", "Debe seleccionar un usuario antes de eliminar", "warning");
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
                ca.DeleteToAPI(urlService, userId, function () {
                    console.log("Usuario eliminado");
                    $('#tblAdmins').DataTable().ajax.reload(); // Recargar la tabla
                });
            }
        });
    };




}

// Inicialización cuando el documento esté listo
$(document).ready(function () {
    if ($("#tblAdmins").length) {
        var vc = new AdminViewController();
        vc.InitView();
    }
});
