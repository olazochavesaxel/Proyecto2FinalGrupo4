function ComisionViewController() {
    this.ViewName = "TablaComision";
    this.ApiEndPointName = "Comision";

    this.InitView = function () {
        console.log("Comision Init View");
        this.LoadTable();
    }

    // Cargar la tabla con datos de administradores desde la API
    this.LoadTable = function () {
        var ca = new ControlActions();
        var service = this.ApiEndPointName + "/RetrieveAll";
        var urlService = ca.GetUrlApiService(service);

        /* th>Id</th>

        th>Cédula</th>
                        <th>Id</th>
                        <th>Tipo</th>
                        <th>Nombre</th>
                        <th>Porcentaje</th>
                        <th>Tarifa 1</th>
                        <th>Tarifa 2</th>
                        <th>Tarifa 3</th>
                        <th>Id Amin</th>
                        
                                {
  "id": 0,
  "created": "2025-03-29T09:49:35.951Z",
  "tipo": "string",
  "nombre": "string",
  "porcentaje": 0,
  "tarifa1": 0,
  "tarifa2": 0,
  "tarifa3": 0,
  "idAdmin": 0
}
                         
          */

        var columns = [];
        columns[0] = { 'data': 'id' };
        columns[1] = { 'data': 'tipo' };
        columns[2] = { 'data': 'nombre' };
        columns[3] = { 'data': 'porcentaje' };
        columns[4] = { 'data': 'tarifa1' };
        columns[5] = { 'data': 'tarifa2' };
        columns[6] = { 'data': 'tarifa3' };
        columns[7] = { 'data': 'idAdmin' };


        $('#tblComision').dataTable({
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
    var vc = new ComisionViewController(); // Corrección del controlador
    vc.InitView();
});



