async function guardarNuevaContrasena() {
    const password = document.getElementById("nuevaContrasena").value;
    const confirmar = document.getElementById("confirmarContrasena").value;
    const mensaje = document.getElementById("mensajeResultado");
    const email = localStorage.getItem("correoOTP");

    if (!email) {
        mensaje.textContent = "No se encontró el correo. Intente de nuevo desde la recuperación.";
        return;
    }

    if (!password || !confirmar) {
        mensaje.textContent = "Por favor, complete ambos campos.";
        return;
    }

    if (password !== confirmar) {
        mensaje.textContent = "Las contraseñas no coinciden.";
        return;
    }

    try {
        const response = await fetch('https://localhost:7057/api/Usuario/actualizar-contrasena', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: email,
                nuevaContrasena: password
            })

        });

        let data = {};
        if (response.headers.get("content-length") !== "0") {
            data = await response.json();
        }

        if (response.ok) {
            mensaje.textContent = "Contraseña actualizada correctamente. Redirigiendo al login...";
            localStorage.removeItem("correoOTP");
            setTimeout(() => {
                window.location.href = "/Login";
            }, 2000);
        } else {
            mensaje.textContent = data.error || "Error al actualizar la contraseña.";
        }

    } catch (error) {
        console.error(error);
        mensaje.textContent = "Error de red: " + error.message;
    }
}
