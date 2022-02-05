 /* Para realizar valdiaciones
    if (document.getElementById("num").value == '') {
        $('#num').siblings('span.error').text('Ingrese el nombre');
        isValid = false;
    } else {
        $('#NumeroMetodoConsignacion').siblings('span.error').text('');
    }
*/

$(document).ready(function () {

    cambiarEstado = (url) => {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (res) {
                alertify.set('notifier', 'position', 'top-right');
                if (res.isValid) {
                    alertify.success("Cambio de estado exitoso");
                } else {
                    if (res.tipoError == "warning") 
                        alertify.alert('Cambiar estado', '¡No se puede cambiar el estado del usuario actual!', function () { location.reload(); });                  
                    else
                        alertify.error("Error al cambiar el estado");
                }
            }
        })
    }

    mostrarModal = (url, title) => {
        //console.log(url); 
        $.ajax({
            type: 'GET',
            url: url,
            success: function (res) {
                if (res.isValid == false) {
                    alertify.set('notifier', 'position', 'top-right');
                    if (res.tipoError == "danger")
                        alertify.error(res.error);
                } else {
                    $('#form-modal .modal-body').html(res);
                    $('#form-modal .modal-title').html(title);
                    $('#form-modal').modal('show');
                }
            }
        })
    }

    mostrarModalLg = (url, title) => {
        //console.log(url); 
        $.ajax({
            type: 'GET',
            url: url,
            success: function (res) {
                if (res.isValid == false) {
                    alertify.set('notifier', 'position', 'top-right');
                    if (res.tipoError == "danger")
                        alertify.error(res.error);
                } else {
                    $('#form-modal-lg .modal-body').html(res);
                    $('#form-modal-lg .modal-title').html(title);
                    $('#form-modal-lg').modal('show');
                }
            }
        })
    }


    //Para guardar
    jQueryAjaxPost = (form, titulo, mensaje) => {

        alertify.confirm(titulo, mensaje,
            function () {
                try {
                    $.ajax({
                        type: 'POST',
                        url: form.action,
                        data: new FormData(form),
                        contentType: false,
                        processData: false,
                        success: function (res) {
                            if (res.isValid) {
                                var mensaje;
                                if (res.operacion == "crear") {
                                    mensaje = "Registro creado exitosamente";
                                }
                                if (res.operacion == "ok") {
                                    mensaje = res.mensaje;
                                }
                                
                                if (res.operacion == "editar") {
                                    mensaje = "Registro editado exitosamente";
                                }

                                alertify.set('notifier', 'position', 'top-right');
                                alertify.notify(mensaje, 'success', 3, function () {
                                    $('#form-modal .modal-body').html('');
                                    $('#form-modal .modal-title').html('');
                                    $('#form-modal').modal('hide');
                                    location.reload();
                                })
                            }
                            else {
                                alertify.set('notifier', 'position', 'top-right');
                                if (res.tipoError == "danger")
                                    alertify.error(res.error);
                                else if (res.tipoError == "warning") {
                                    alertify.warning(res.error);
                                    $('#form-modal .modal-body').html(res.html);
                                } else if (res.tipoError == "password") {
                                    alertify.alert('Error en la contraseña', res.error);
                                } else if (res.tipoError == "correo") {
                                    alertify.alert('No se pudo crear el usuario', res.error);
                                }
                            }
                        },
                        error: function (err) {
                            console.log(err)
                        }
                    })
                } catch (ex) {
                    console.log(ex)
                }
            },
            function () {
                alertify.error('Cancelado');
            }).set('labels', { ok: 'Guardar', cancel: 'Cancelar' }).set('notifier', 'position', 'top-right');


        //to prevent default form submit event
        return false;
    } 

    
});
//Avatar  
(function (w, d) {


    function LetterAvatar(name, size) {

        name = name || '';
        size = size || 60;

        var colours = [
            "#1abc9c", "#2ecc71", "#3498db", "#9b59b6", "#34495e", "#16a085", "#27ae60", "#2980b9", "#8e44ad", "#2c3e50",
            "#f1c40f", "#e67e22", "#e74c3c", "#ecf0f1", "#95a5a6", "#f39c12", "#d35400", "#c0392b", "#bdc3c7", "#7f8c8d"
        ],

            nameSplit = String(name).toUpperCase().split(' '),
            initials, charIndex, colourIndex, canvas, context, dataURI;


        if (nameSplit.length == 1) {
            initials = nameSplit[0] ? nameSplit[0].charAt(0) : '?';
        } else {
            initials = nameSplit[0].charAt(0) + nameSplit[1].charAt(0);
        }

        if (w.devicePixelRatio) {
            size = (size * w.devicePixelRatio);
        }

        charIndex = (initials == '?' ? 72 : initials.charCodeAt(0)) - 64;
        colourIndex = charIndex % 20;
        canvas = d.createElement('canvas');
        canvas.width = size;
        canvas.height = size;
        context = canvas.getContext("2d");

        context.fillStyle = colours[colourIndex - 1];
        context.fillRect(0, 0, canvas.width, canvas.height);
        context.font = Math.round(canvas.width / 2) + "px Arial";
        context.textAlign = "center";
        context.fillStyle = "#FFF";
        context.fillText(initials, size / 2, size / 1.5);

        dataURI = canvas.toDataURL();
        canvas = null;

        return dataURI;
    }

    LetterAvatar.transform = function () {

        Array.prototype.forEach.call(d.querySelectorAll('img[avatar]'), function (img, name) {
            name = img.getAttribute('avatar');
            img.src = LetterAvatar(name, img.getAttribute('width'));
            img.removeAttribute('avatar');
            img.setAttribute('alt', name);
        });
    };


    // AMD support
    if (typeof define === 'function' && define.amd) {

        define(function () { return LetterAvatar; });

        // CommonJS and Node.js module support.
    } else if (typeof exports !== 'undefined') {

        // Support Node.js specific `module.exports` (which can be a function)
        if (typeof module != 'undefined' && module.exports) {
            exports = module.exports = LetterAvatar;
        }

        // But always support CommonJS module 1.1.1 spec (`exports` cannot be a function)
        exports.LetterAvatar = LetterAvatar;

    } else {

        window.LetterAvatar = LetterAvatar;

        d.addEventListener('DOMContentLoaded', function (event) {
            LetterAvatar.transform();
        });
    }

})(window, document);