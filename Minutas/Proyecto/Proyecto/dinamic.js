let modal = document.querySelector("#modal");
    let eliminar = document.querySelector("table");
    eliminar.addEventListener("click", function(e){
        if(e.target.tagName === "TD" && e.target.id === "eliminar"){
            modal.style.display = "block";
        }
    });
    document.querySelector("#modal").addEventListener("click", function(e){
        if(e.target.tagName === "INPUT"){
            modal.style.display = "none";
        }
        
    });