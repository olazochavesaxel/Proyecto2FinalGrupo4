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

    fetch(`https://localhost:7057/api/Asesor/RetrieveById?id=${user.id}`)
        .then(response => {
            if (!response.ok) throw new Error("Error al obtener los datos del asesor");
            return response.json();
        })
        .then(asesor => {
            console.log("Asesor recibido del backend:", asesor);

            document.getElementById("asesorNombre").innerText = `${asesor.nombre} ${asesor.primerApellido} ${asesor.segundoApellido}`;
            document.getElementById("asesorCorreo").innerText = asesor.correo;

            const ingreso = asesor.ingresoComisiones ?? 0;
            document.getElementById("IngresoComisiones").textContent =
                `${ingreso.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })} $`;

            document.getElementById("btnPayPal").addEventListener("click", () => {
                const urlPayPal = `https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=tu-correo-paypal@example.com&item_name=Pago Asesor&amount=${ingreso.toFixed(2)}&currency_code=USD`;
                window.location.href = urlPayPal;
            });
        })
        .catch(error => {
            console.error(error);
            alert("Hubo un problema al cargar los datos del asesor.");
        });
});
