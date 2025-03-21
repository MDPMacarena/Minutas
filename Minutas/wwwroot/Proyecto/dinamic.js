/*let modal = document.querySelector("#modal");
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
        
    });*/
/*
    let nav = document.querySelector("nav");
    nav.addEventListener("click", function(e){
        if(e.target.tagName == "A"){
            let div = e.target.parentElement;
            let color = "blue";
    
            localStorage.setItem("pestanaSeleccionada", e.target.href);
            localStorage.setItem("colorFondo", nuevoColor);
    
            window.location.href = e.target.href;
        }
    });*/

    let nav = document.querySelector("nav a");
    let div = document.querySelector("nav div");
    nav.addEventListener("click", function(e){
        if(e.target.tagName == "A"){
            div.style.background="blue";
        }
    });