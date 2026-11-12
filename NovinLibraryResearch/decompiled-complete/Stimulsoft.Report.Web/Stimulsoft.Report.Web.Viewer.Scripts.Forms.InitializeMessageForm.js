
StiJsViewer.prototype.InitializeErrorMessageForm = function () {
    var form = this.BaseForm("errorMessageForm", this.collections.loc["Error"], 4);
    form.buttonCancel.style.display = "none";
    
    var table = this.CreateHTMLTable();
    form.container.appendChild(table);

    form.image = document.createElement("img");
    form.image.style.boxSizing = "content-box";
    form.image.style.padding = "15px";
    form.image.style.width = form.image.style.height = "32px";
    form.image.src = this.collections.images["MsgFormError.png"];
    table.addCellInLastRow(form.image);

    form.description = table.addCellInLastRow();
    form.description.className = "stiJsViewerMessagesFormDescription";
    form.description.style.maxWidth = "600px";
    form.description.style.color = this.options.toolbar.fontColor;

    form.show = function (messageText, messageType) {
        if (this.visible) {
            this.description.innerHTML += "<br/><br/>" + messageText;
            return;
        }

        if (this.jsObject.controls.forms.errorMessageForm) { //Fixed Bug
            this.jsObject.controls.mainPanel.removeChild(this.jsObject.controls.forms.errorMessageForm);
            this.jsObject.controls.mainPanel.appendChild(this.jsObject.controls.forms.errorMessageForm);
        }

        this.caption.innerHTML = this.jsObject.collections.loc["FormViewerTitle"];

        if (messageType == "Warning") this.image.src = this.jsObject.collections.images["MsgFormWarning.png"];
        else if (messageType == true || messageType == "Info") this.image.src = this.jsObject.collections.images["MsgFormInfo.png"]; //messageType === true - for backward compatibility
        else {
            this.image.src = this.jsObject.collections.images["MsgFormError.png"];
            this.caption.innerHTML = this.jsObject.collections.loc["Error"];
        }

        this.changeVisibleState(true);
        this.description.innerHTML = messageText;
    }

    form.action = function () {
        this.changeVisibleState(false);
    }

    return form;
}