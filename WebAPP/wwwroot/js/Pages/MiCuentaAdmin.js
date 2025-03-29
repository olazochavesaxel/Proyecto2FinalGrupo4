//JS que manejaa todo el comportamiento de la vista de usuarios, o Users.cshtml

//Definir una clase en JS, usando prototype
//patron : MVC. Modelo, vista, controlador

function AdminTableViewController() {

    this.viewName = "Users";
    this.ApiEndPointName = "Admin";

    //Metodo "contructor" de la vista
    this.InitView = function () {

        console.log("User init view");
        this.LoadTable();

        //Definir que el boton de create debe llamar al metodo
        $("#btnCreate").click(function () {
            var vc = new AdminTableViewController();
            vc.Create();
        })
    }

    //Metodo que se encarga del llenado de la tabla

    this.LoadTable = function () {

        //URL a invocar http://localhost:5207/api/Product/RetrieveAll
        var ca = new ControlActions();
        var service = this.ApiEndPointName + "/RetrieveById";

        var urlService = ca.GetUrlApiService(service);

        /*                      
    "cedula": "12345678",
    "nombre": "Juan",
    "primerApellido": "Perez",
    "segundoApellido": "Lopez",
    "direccion": "San José, Costa Rica",
    "fotoPerfil": "foto_perfil.jpg",
    "contrasenna": "hashed_password_123",
    "telefono": "8888-8888",
    "estado": "Activo",
    "rol": "Admin",
    "fechaNacimiento": "1990-05-15T00:00:00",
    "fechaExpiracionOTP": "2025-04-01T00:00:00",
    "correo": "juan.perez@example.com",
    "id": 6,
    "created": "2025-03-26T18:38:28.887"
  
*/



        var columns = [];
        columns[0] = { 'data': 'cedula' }
        columns[1] = { 'data': 'nombre' }
        columns[2] = { 'data': 'primerApellido' }
        columns[3] = { 'data': 'segundoApellido' }
        columns[4] = { 'data': 'direccion' }
        columns[5] = { 'data': 'fotoPerfil' }
        columns[6] = { 'data': 'contrasenna' }
        columns[7] = { 'data': 'telefono' }
        columns[8] = { 'data': 'estado' }
        columns[9] = { 'data': 'rol' }
        columns[10] = { 'data': 'fechaNacimiento' }
        columns[11] = { 'data': 'correo' }
        columns[12] = { 'data': 'id' }
        columns[13] = { 'data': 'created' }

        $('#tblUsers').dataTable({
            "ajax": {
                "url": urlService,
                "dataSrc": ""

            },
            columns: columns

        });

        //asignar eventos de carga de datos en el click de la tabla (binding de data)

        $('#tblUsers tbody').on('click', 'tr', function () {

            //Extraemos la fila
            var row = $(this).closest('tr');

            //extraer el DTO
            var userDTO = $('#tblUsers').dataTable().row(row).data();

            //Mapeo con el formulario
            $('#txtId').val(userDTO.id);
            $('#txtUserCode').val(userDTO.userCode);
            $('#txtName').val(userDTO.name);
            $('#txtLastName').val(userDTO.lastName);
            $('#txtEmail').val(userDTO.email);
            $('#txtPhone').val(userDTO.phoneNumber);

            //Fecha tiene un formato
            var onlyDate = userDTO.birthDate.split("T")
            $('#txtBirthDate').val(onlyDate[0]);


        })
    }

    //Metodo para create

    this.Create = function () {

        //Creamos el DTO
        var userDTO = {}
        //Atributos con valores default, controlados por el API
        userDTO.userCode = "default";
        userDTO.id = 0;
        userDTO.created = "2025-02-14T02:34:25.71";

        //Valores definidos por el usuario.
        userDTO.name = $("#txtName").val();
        userDTO.lastName = $("#txtLastName").val();
        userDTO.email = $("#txtEmail").val();
        userDTO.phoneNumber = $("#txtPhone").val();
        userDTO.birthDate = $("#txtBirthDate").val();
        userDTO.password = $("#txtPassword").val();

        //invocar al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Create";

        ca.PostToAPI(urlService, userDTO, function () {
            console.log("User created");

            //Recargar la tabla despues de creado el registro
            $('#tblUsers').dataTable().ajax.reload();



        })
    }
}




$(document).ready(function () {
    var vc = new AdminTableViewController();
    vc.InitView();

})

