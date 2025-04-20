document.addEventListener('DOMContentLoaded', function () {
    const inputs = document.querySelectorAll('.otp-inputs input');
    const timerDisplay = document.getElementById('otp-timer');
    const btnConfirmar = document.querySelector('.btn-confirmar');
    let otpTimeout;
    let timeLeft = 60; // segundos

    // 🕒 Inicia el temporizador
    startTimer();

    // 👉 Enfocar automáticamente el siguiente input
    inputs.forEach((input, index) => {
        input.addEventListener('input', function () {
            if (input.value.length === 1 && index < inputs.length - 1) {
                inputs[index + 1].focus();
            }
        });

        input.addEventListener('keydown', function (e) {
            if (e.key === 'Backspace' && input.value === '' && index > 0) {
                inputs[index - 1].focus();
            }
        });
    });

    // ✅ Botón confirmar
    btnConfirmar.addEventListener('click', async function () {
        const otpCode = Array.from(inputs).map(i => i.value).join('');

        if (otpCode.length < 6) {
            Swal.fire('Código incompleto', 'Por favor completá los 6 dígitos.', 'warning');
            return;
        }

        // Acá iría el fetch al backend, pero simulamos validación
        const esValido = await validarOTP(otpCode);

        if (esValido) {
            Swal.fire({
                icon: 'success',
                title: 'Código correcto',
                text: 'En un momento estableceras tu nueva contraseña...',
                timer: 2000,
                showConfirmButton: false
            }).then(() => {
                const origen = localStorage.getItem("origenOTP");

                if (origen === "recuperacion") {
                    window.location.href = "/NuevaContrasenna";
                } else {
                    window.location.href = "/Login"; // o donde quieras enviarlo luego de registrarse
                }
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Código incorrecto',
                text: 'Se ha generado un nuevo código.'
            });
            reiniciarInputs();
            reiniciarTimer();
        }
    });

    function startTimer() {
        updateTimerDisplay();
        otpTimeout = setInterval(() => {
            timeLeft--;
            updateTimerDisplay();

            if (timeLeft <= 0) {
                clearInterval(otpTimeout);
                Swal.fire('Código expirado', 'Se ha generado un nuevo código.', 'info');
                reiniciarInputs();
                reiniciarTimer();
            }
        }, 1000);
    }

    function updateTimerDisplay() {
        const minutes = String(Math.floor(timeLeft / 60)).padStart(2, '0');
        const seconds = String(timeLeft % 60).padStart(2, '0');
        timerDisplay.textContent = `${minutes}:${seconds}`;
    }

    function reiniciarInputs() {
        inputs.forEach(i => i.value = '');
        inputs[0].focus();
    }

    function reiniciarTimer() {
        clearInterval(otpTimeout);
        timeLeft = 120;
        startTimer();
    }

    async function validarOTP(code) {
        const emailUsuario = localStorage.getItem("correoOTP");

        try {
            const response = await fetch('https://localhost:7057/api/EmailVerification/verificar-otp', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    Correo: emailUsuario,
                    Codigo: code
                })

            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.mensaje || 'Error desconocido');
            }

            return true;
        } catch (error) {
            console.error('Error validando OTP:', error.message);
            Swal.fire({
                icon: 'error',
                title: 'Código incorrecto',
                text: error.message
            });
            return false;
        }
    }

});
