document.addEventListener("DOMContentLoaded", () => {
    const rolSelect = document.getElementById("rol");
    const clienteFields = document.getElementById("clienteFields");
    const asesorFields = document.getElementById("asesorFields");
    const form = document.getElementById("registerForm");

    // Mostrar u ocultar campos según el rol
    rolSelect.addEventListener("change", () => {
        const rol = rolSelect.value;
        clienteFields.style.display = rol === "Cliente" ? "block" : "none";
        asesorFields.style.display = rol === "Asesor" ? "block" : "none";
    });

    // Envío del formulario
    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        const rol = document.getElementById("rol").value;

        const datos = {
            cedula: document.getElementById("cedula").value,
            nombre: document.getElementById("nombre").value,
            primerApellido: document.getElementById("primerApellido").value,
            segundoApellido: document.getElementById("segundoApellido").value,
            direccion: document.getElementById("direccion").value,
            telefono: document.getElementById("telefono").value,
            fechaNacimiento: document.getElementById("fechaNacimiento").value,
            correo: document.getElementById("email").value,
            contrasenna: document.getElementById("password").value,
            estado: "Pendiente",
            fechaExpiracionOTP: new Date().toISOString(),
            created: new Date().toISOString(),
            rol: rol
        };

        // Agregar campos según el rol
        if (rol === "Cliente") {
            datos.balanceFinanciero = parseFloat(document.getElementById("balanceFinanciero").value) || 0;
        }

        if (rol === "Asesor") {
            datos.ingresoComisiones = parseFloat(document.getElementById("ingresoComisiones").value) || 0;
            datos.clientes = [];
        }

        // Subir la imagen si existe
        const fotoPerfilInput = document.getElementById("fotoPerfil");
        if (fotoPerfilInput && fotoPerfilInput.files.length > 0) {
            const imagenForm = new FormData();
            imagenForm.append("archivo", fotoPerfilInput.files[0]);

            try {
                const uploadResp = await fetch("https://localhost:7057/api/Cliente/UploadFotoPerfil", {
                    method: "POST",
                    body: imagenForm
                });

                const resultado = await uploadResp.json();

                if (uploadResp.ok && resultado.url) {
                    datos.fotoPerfil = resultado.url;
                } else {
                    console.error("Error al obtener la URL de la imagen:", resultado);
                    datos.fotoPerfil = "";
                }
            } catch (error) {
                console.error("Error al subir imagen:", error);
                datos.fotoPerfil = "";
            }
        } else {
            datos.fotoPerfil = "";
        }

        // Enviar el objeto al backend
        const endpoint = rol === "Cliente" ? "Cliente/Create" : "Asesor/Create";

        try {
            const response = await fetch(`https://localhost:7057/api/${endpoint}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(datos)
            });

            if (!response.ok) throw new Error("Error al crear usuario");

            // Llamar al endpoint que envía el correo OTP
            await fetch("https://localhost:7057/api/EmailVerification/verificar", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ correo: datos.correo })
            });

            // Luego sí redirigir
            localStorage.setItem("correoOTP", datos.correo);
            localStorage.setItem("origenOTP", "registro");
            window.location.href = "/AutentificacionOTP";

        } catch (err) {
            console.error(err);
            alert("Error al registrar el usuario.");
        }
    });
});
