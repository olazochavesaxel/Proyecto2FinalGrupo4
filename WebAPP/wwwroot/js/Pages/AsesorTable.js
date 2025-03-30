function AsesorViewController() {

    this.ViewName = "TablaAsesor";
    this.ApiEndPointName = "Asesor";

    this.InitView = function () {
        console.log("Asesor Init View");
        this.LoadTable();
    }

    // Cargar la tabla con datos de administradores desde la API
    this.LoadTable = function () {
        var ca = new ControlActions();
        var service = this.ApiEndPointName + "/RetrieveAll";
        var urlService = ca.GetUrlApiService(service);

        /* th>Id</th>
                                <th>Cédula</th>
                                <th>Nombre</th>
                                <th>Primer Apellido</th>
                                <th>Segundo Apellido</th>
                                <th>Dirección</th>
                                <th>Telefono</th>
                                <th>Estado</th>
                                <th>Rol</th>
                                <th>Fecha de Nacimiento</th>
                                <th>Correo Electrónico</th>
                                <th>Fecha  Creación</th>
                                <th>Balance Financiero</th>
                                https://localhost:7057/api/CLiente/RetrieveAll 
                         
          */

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
        columns[9] = { 'data': 'fechaNacimiento' }; // Corrección del nombre
        columns[10] = { 'data': 'correo' };
        columns[11] = { 'data': 'created' };
        columns[12] = { 'data': 'ingresoComisiones' };

        $('#tblAsesores').dataTable({
            "ajax": {
                "url": urlService,
                "dataSrc": ""
            },
            columns: columns // Corrección en el nombre de la propiedad
        });
    }
}

// $ referencia a jQuery
$(document).ready(function () {
    var vc = new AsesorViewController(); // Corrección del controlador
    vc.InitView();
});


