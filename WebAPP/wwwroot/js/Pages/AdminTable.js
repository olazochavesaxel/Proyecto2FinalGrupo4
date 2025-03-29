
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
            let userDTO = {};
            userDTO.id = $("#txtId").val();
            userDTO.nombre = $("#txtNombre").val();
            userDTO.cedula = $("#txtCedula").val();
            // Agrega los demás campos

            console.log(userDTO); // Solo para verificar que los datos están correctos antes de enviarlos

            // Aquí iría tu lógica para enviar los datos al backend
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

                // Formato de fecha
                var onlyDate = userDTO.fechaNacimiento ? userDTO.fechaNacimiento.split("T")[0] : "";
                $('#txtBirthDate').val(onlyDate);
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
            //Recargar la tabla despues de crar el registro
            $('#tblAdmins').DataTable().ajax.reload(); // CORRECCIÓN: Usar #tblAdmins
        });
    }

    //Metodo UPDATE
   
    this.Update = function () {
        var userDTO = {};
        userDTO.id = $("#txtId").val(); // Asegúrate de tener un campo hidden con el ID del usuario

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

        // Verifica que los datos sean correctos
        console.log(userDTO);

        // Invocar API con PUT
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Update/" + userDTO.id;  // Usa el ID en la URL

        ca.PutToAPI(urlService, userDTO, function (response) {
            console.log("Usuario actualizado");
            $('#tblAdmins').DataTable().ajax.reload();
        });
    };


}

// Inicialización cuando el documento esté listo
$(document).ready(function () {
    var vc = new AdminViewController();
    vc.InitView();
});
