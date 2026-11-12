
StiJsViewer.prototype.GroupPanel = function (caption, isOpened, width, innerPadding) {
    var groupPanel = document.createElement("div");
    groupPanel.style.fontFamily = this.options.toolbar.fontFamily;
    groupPanel.style.color = this.options.toolbarFontColor;
    groupPanel.jsObject = this;
    if (width) groupPanel.style.minWidth = width + "px";
    groupPanel.style.overflow = "hidden";
    groupPanel.isOpened = isOpened;
    var header = this.FormButton(null, caption, isOpened ? "Arrows.BigArrowDown.png" : "Arrows.BigArrowRight.png");
    header.imageCell.style.width = "1px";
    if (header.caption) {
        header.caption.style.textAlign = "left";
        header.caption.style.padding = "0 15px 0 5px";
    }

    groupPanel.appendChild(header);
    var container = document.createElement("div");
    if (innerPadding) container.style.padding = innerPadding;
    container.style.display = isOpened ? "" : "none";
    container.className = "stiJsViewerGroupPanelContainer";
    groupPanel.container = container;
    groupPanel.appendChild(container);

    groupPanel.changeOpeningState = function (state) {
        groupPanel.isOpened = state;
        header.image.src = groupPanel.jsObject.collections.images[state ? "Arrows.BigArrowDown.png" : "Arrows.BigArrowRight.png"];
        container.style.display = state ? "" : "none";
    }

    header.action = function () {
        groupPanel.isOpened = !groupPanel.isOpened;
        header.image.src = groupPanel.jsObject.collections.images[groupPanel.isOpened ? "Arrows.BigArrowDown.png" : "Arrows.BigArrowRight.png"];
        groupPanel.style.height = (groupPanel.isOpened ? header.offsetHeight : header.offsetHeight + container.offsetHeight) + "px";
        if (groupPanel.isOpened) container.style.display = "";
        groupPanel.jsObject.animate(groupPanel, {
            duration: 150,
            animations: [{
                style: "height",
                start: groupPanel.isOpened ? header.offsetHeight : header.offsetHeight + container.offsetHeight,
                end: groupPanel.isOpened ? header.offsetHeight + container.offsetHeight : header.offsetHeight,
                postfix: "px",
                finish: function () {
                    container.style.display = groupPanel.isOpened ? "" : "none";
                    groupPanel.style.height = "";
                }
            }]
        });
    }

    return groupPanel;
}