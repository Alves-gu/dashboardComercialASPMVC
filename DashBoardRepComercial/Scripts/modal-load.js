window.onload = function () {
    //Obtenção de elementos html a serem manipulados
    var modal = document.getElementById('myModal');

    var form;

    //Gatilho p/ modal da view Aviso/Create
    if (document.getElementById("formUploadAviso")) {
        formUploadAviso.onsubmit = function formAviso() {
            modal.style.display = "block";
        }
    }

    //Gatilho de função para submissão de formulário
    if (form = document.getElementById("formUploadDados")) {
        form.onsubmit = function validarForm() {
            fileInput = document.getElementById("fileField");
            //Validação de tamanho de arquivo
            if (fileInput.files[0].size > 4194300) {
                event.preventDefault();
                alert("Por favor, selecione um arquivo com tamanho inferior a 4mb.");
                window.location.reload();
                return false;
            }
            else {
                //Se o arquivo for menor que 4mb, exibir modal de carregamento e liberar postagem de formulário.
                modal.style.display = "block";
                return true;
            }
        }
    }
    
    if (document.getElementById("pop")) {
        $("#pop").on("click", function () {
            $('#imagepreview').attr('src', $('#imageresource').attr('src')); // here asign the image to the modal when the user click the enlarge link
            $('#imagemodal').modal('show'); // imagemodal is the id attribute assigned to the bootstrap modal, then i use the show function
        });
    }
}