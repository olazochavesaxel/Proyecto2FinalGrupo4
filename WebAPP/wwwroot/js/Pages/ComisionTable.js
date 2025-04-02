
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
        console.log("🔹 ControlActions cargado:", ca); // 🔹 Verifica si ControlActions está definido

        var service = this.ApiEndPointName + "/RetrieveAll";
        var urlService = ca.GetUrlApiService(service);

        console.log("URL de la API:", urlService); // 🔹 Verifica la URL generada

        // Verifica si la API devuelve datos
        fetch(urlService)
            .then(response => response.json())
            .then(data => console.log("Datos de la API:", data)) // 🔹 Muestra los datos de la API en la consola
            .catch(error => console.error("Error al obtener datos:", error));

        var columns = [];
        columns[0] = { 'data': 'id' };
        columns[1] = { 'data': 'tipo' };
        columns[2] = { 'data': 'nombre' };
        columns[3] = { 'data': 'porcentaje' };
        columns[4] = { 'data': 'tarifa1' };
        columns[5] = { 'data': 'tarifa2' };
        columns[6] = { 'data': 'tarifa3' };
        columns[7] = { 'data': 'idAdmin' };

        $('#tblComision').DataTable({
            "destroy": true, // 🔹 Permite recargar la tabla sin errores
            "ajax": {
                "url": urlService,
                "dataSrc": ""
            },
            columns: columns
        });
    };

}

// $ referencia a jQuery
$(document).ready(function () {
    console.log("Document Ready - Inicializando ComisionViewController");
    var vc = new ComisionViewController();
    vc.InitView();
});



