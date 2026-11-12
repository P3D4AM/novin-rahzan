
StiJsViewer.prototype.InitializeSendEmailForm = function (form) {
    var sendEmailForm = this.BaseForm("sendEmailForm", this.collections.loc["EmailOptions"], 1);
    sendEmailForm.style.fontFamily = this.options.toolbar.fontFamily;
    if (this.options.toolbar.fontColor != "") sendEmailForm.style.color = this.options.toolbar.fontColor;
    sendEmailForm.style.fontSize = "12px";
    sendEmailForm.controls = {};

    var controlsWidth = this.options.isMobileDevice ? 200 : 280;
    var controlProps = [
        ["Email", this.collections.loc["Email"], this.TextBox("sendEmailFormEmail", controlsWidth)],
        ["Subject", this.collections.loc["Subject"], this.TextBox("sendEmailFormSubject", controlsWidth)],
        ["Message", this.collections.loc["Message"], this.TextArea("sendEmailFormMessage", controlsWidth, 70)],
        ["AttachmentCell", this.collections.loc["Attachment"], document.createElement("div")]
    ]

    var controlsTable = this.CreateHTMLTable();
    sendEmailForm.container.appendChild(controlsTable);

    for (var i = 0; i < controlProps.length; i++) {
        var control = controlProps[i][2];
        control.style.margin = this.options.isMobileDevice ? "4px 4px 12px 4px" : "4px";
        sendEmailForm.controls[controlProps[i][0]] = control;
        var textCell = controlsTable.addTextCellInLastRow(controlProps[i][1]);
        textCell.className = "stiJsViewerCaptionControls";
        controlsTable[this.options.isMobileDevice ? "addCellInNextRow" : "addCellInLastRow"](control);
        if (this.options.isMobileDevice) textCell.style.padding = "0 25px 0 4px";
        if (i < controlProps.length - 1) controlsTable.addRow();
    }
    
    sendEmailForm.show = function (exportFormat, exportSettings) {
        this.changeVisibleState(true);
        this.exportSettings = exportSettings;
        this.exportFormat = exportFormat;

        for (var i in this.controls) {
            this.controls[i].value = "";
        }

        this.controls["Email"].value = this.jsObject.options.email.defaultEmailAddress;
        this.controls["Message"].value = this.jsObject.options.email.defaultEmailMessage;
        this.controls["Subject"].value = this.jsObject.options.email.defaultEmailSubject;

        var ext = this.exportFormat.toLowerCase().replace("image", "");
        switch (ext) {
            case "excel": ext = "xls"; break;
            case "excel2007": ext = "xlsx"; break;
            case "excelxml": ext = "xls"; break;
            case "html5": ext = "html"; break;
            case "jpeg": ext = "jpg"; break;
            case "ppt2007": ext = "ppt"; break;
            case "text": ext = "txt"; break;
            case "word2007": ext = "docx"; break;
        }

        this.controls["AttachmentCell"].innerHTML = this.jsObject.reportParams.reportFileName + "." + ext;
    }

    sendEmailForm.action = function () {
        sendEmailForm.exportSettings["Email"] = sendEmailForm.controls["Email"].value;
        sendEmailForm.exportSettings["Subject"] = sendEmailForm.controls["Subject"].value;
        sendEmailForm.exportSettings["Message"] = sendEmailForm.controls["Message"].value;

        sendEmailForm.changeVisibleState(false);
        sendEmailForm.jsObject.postEmail(sendEmailForm.exportFormat, sendEmailForm.exportSettings);
    }
}