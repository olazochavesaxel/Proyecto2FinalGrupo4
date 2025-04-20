async function enviarOtp() {
    const email = document.getElementById("correoRecuperacion").value;
    const mensaje = document.getElementById("mensajeRespuesta");

    if (!email) {
        mensaje.textContent = "Por favor, ingrese su correo.";
        return;
    }

    try {
        const response = await fetch('https://localhost:7057/api/EmailVerification/verificar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ correo: email })
        });


        const data = await response.json();

        if (response.ok) {
            localStorage.setItem("correoOTP", email);
            localStorage.setItem("origenOTP", "recuperacion");
            window.location.href = "/AutentificacionOTP";
        } else {
            mensaje.textContent = data.error || "Ocurrió un error al enviar el código.";
        }
    } catch (error) {
        mensaje.textContent = "Error de red: " + error.message;
    }
}
