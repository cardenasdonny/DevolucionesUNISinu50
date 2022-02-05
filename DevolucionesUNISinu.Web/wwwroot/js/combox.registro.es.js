/*
$(function () {
    $("#FacultadId").change(function () {
        var url = '@Url.Content("~/")' + "Estudiantes/ObtenerProgramasPorFacultad";
        var idFacultad = "#FacultadId";
        $.getJSON(url, { id: $(idFacultad).val() }, function (data) {
            var items = "";
            $("#ProgramaId").empty();
            $.each(data, function (i, row) {
                items += "<option value='" + row.value + "'>" + row.text + "</option>";
            });
            $("#ProgramaId").html(items);
        });
    });
    */

$(function () {
    $("#FacultadId").change(function () {
        var url = '@Url.Content("~/")' + "Estudiantes/ObtenerProgramasPorFacultad";
        var idFacultad = "#FacultadId";
        $.getJSON(url, { id: $(idFacultad).val() }, function (data) {
            var items = "";
            $("#ProgramaId").empty();
            $.each(data, function (i, row) {
                items += "<option value='" + row.value + "'>" + row.text + "</option>";
            });
            $("#ProgramaId").html(items);
        });
    });


    $("#ProgramaId").change(function () {
        var url = '@Url.Content("~/")' + "Estudiantes/ObtenerSemestresPorPrograma";
        var idFacultad = "#ProgramaId";
        $.getJSON(url, { id: $(idFacultad).val() }, function (data) {
            var items = "";
            $("#SemestreId").empty();
            $.each(data, function (i, row) {
                items += "<option value='" + row.value + "'>" + row.text + "</option>";
            });
            $("#SemestreId").html(items);
        });
    });
});
