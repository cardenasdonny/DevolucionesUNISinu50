
        Dropzone.options.dropzoneForm = {
            url: "/Devoluciones/Crear",
            
            paramName: "formulario",
            autoProcessQueue: false,
            uploadMultiple: true,
            parallelUploads: 5,
            maxFiles: 5,
            maxFilesize: 10, // MB
            dictDefaultMessage: 'Adjuntar documentos',
            acceptedFiles: "image/jpeg, image/png,image/jpg, application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/msword, application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            init: function () {

                var submitButton = document.querySelector("#submit-all");
                var wrapperThis = this;

                submitButton.addEventListener("click", function () {
                    wrapperThis.processQueue();
                });

                this.on("addedfile", function (file) {

                    // Create the remove button
                    var removeButton = Dropzone.createElement("<button class='btn btn-lg dark'>Eliminar archivo</button>");

                    // Listen to the click event
                    removeButton.addEventListener("click", function (e) {
                        // Make sure the button click doesn't submit the form:
                        e.preventDefault();
                        e.stopPropagation();

                        // Remove the file preview.
                        wrapperThis.removeFile(file);
                        // If you want to the delete the file on the server as well,
                        // you can do the AJAX request here.
                    });

                    // Add the button to the file preview element.
                    file.previewElement.appendChild(removeButton);
                });
                this.on("sending", function (file, response, formData) {
                    formData.append("NumeroRadicado", $("#NumeroRadicado").val());
                    formData.append("Files", file);
                });

                this.on("success", function (file, response) {
                    //var obj = jQuery.parseJSON(response)
                    //console.log(response);
                    alertify.set('notifier', 'position', 'top-right');
                    if (response.respuesta == 1) {
                        alertify.notify(response.mensaje, 'success', 2, function () {
                            location.reload();
                        })
                    } else if (response.respuesta == 2) {
                        alertify.notify(response.mensaje, 'warning', 2, function () {
                            console.log(response.html)
                            $('#form-modal .modal-body-p').html(response.html);

                        })
                    } else if (response.respuesta == 3) {
                        alertify.notify("error", 'error', 2, function () {

                        })
                    }
                })
                this.on('error', function (file, response) {
                    console.log(response.html)
                    alertify.set('notifier', 'position', 'top-right');
                    alertify.notify("Error", 'error', 2, function () {                       
                    })
                });

                //this.on('sendingmultiple', function (data, xhr, formData) {
                //    formData.append("Name", $("#Name").val());
                //});
            }
        };
