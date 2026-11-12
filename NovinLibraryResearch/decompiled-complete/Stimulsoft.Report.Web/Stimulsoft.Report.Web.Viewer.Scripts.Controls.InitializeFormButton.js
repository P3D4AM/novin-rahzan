
StiJsViewer.prototype.FormButton = function (name, caption, imageName, minWidth) {
    var button = this.SmallButton(name, caption || "", imageName, null, null, "stiJsViewerFormButton");
    button.innerTable.style.width = "100%";
    button.style.minWidth = (minWidth || 80) + "px";
    button.caption.style.textAlign = "center";
    
    return button;
}