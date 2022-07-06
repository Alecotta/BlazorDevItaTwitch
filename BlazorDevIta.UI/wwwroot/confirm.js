var myModal = {};

//I metodi devono essere agganciati all'oggetto window.
export function showConfirm (id) {
    myModal[id] = new bootstrap.Modal('#' + id);
    myModal[id].show();
}

//Export per rispettare lo standard ECMA.
export function hideConfirm (id) {
    myModal[id].hide();
}