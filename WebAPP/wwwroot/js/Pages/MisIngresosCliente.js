document.addEventListener("DOMContentLoaded", () => {
    const usuarioData = sessionStorage.getItem("usuario");

    if (!usuarioData) {
        alert("Sesión no encontrada. Por favor, inicia sesión nuevamente.");
        window.location.href = "/login";
        return;
    }

    let user;
    try {
        user = JSON.parse(usuarioData);
    } catch (e) {
        sessionStorage.clear();
        alert("Sesión inválida. Por favor, inicia sesión nuevamente.");
        window.location.href = "/login";
        return;
    }

    // Llamada al backend usando query string ?id=
    fetch(`https://localhost:7057/api/CLiente/RetrieveById?id=${user.id}`)

        .then(response => {
            if (!response.ok) throw new Error("Error al obtener los datos del cliente");
            return response.json();
        })
        .then(cliente => {
            document.getElementById("clienteNombre").innerText = `${cliente.nombre} ${cliente.primerApellido} ${cliente.segundoApellido}`;
            document.getElementById("clienteCorreo").innerText = cliente.correo;
            document.getElementById("clienteBalance").textContent =
            `${cliente.balanceFinanciero.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })} $`;


            document.getElementById("btnPayPal").addEventListener("click", () => {
                const urlPayPal = `https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=tu-correo-paypal@example.com&item_name=Pago Cliente&amount=${cliente.balanceFinanciero.toFixed(2)}&currency_code=USD`;
                window.location.href = urlPayPal;
            });
        })
        .catch(error => {
            console.error(error);
            alert("Hubo un problema al cargar los datos del cliente.");
        });
});
