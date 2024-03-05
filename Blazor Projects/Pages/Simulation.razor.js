"use strict";

var canvas = document.getElementById("tile-canvas");
var context = canvas.getContext("2d");

var tileSize;
var canvasWidth;
var canvasHeight;

var amountOfRows;
var amountOfColumns;
var dotnetInstance;

registerWindowResizeHandler();
registerClickHandler();

function registerWindowResizeHandler() {
    addEventListener("resize", (e) => { handleWindowResize() });
}

function registerClickHandler() {
    canvas.addEventListener("click", (e) => { handleOnClick(e) });
}

function handleWindowResize() {
    
}

function handleOnClick(e) {
    var tile = canvas.getBoundingClientRect();
    var tileX = e.clientX - tile.left;
    var tileY = e.clientY - tile.top;

    var clickedRow = Math.floor(tileY / tileSize);
    var clickedCol = Math.floor(tileX / tileSize);

    var clickedTile = { X: clickedCol * tileSize, Y: clickedRow * tileSize, Checked: true};

    dotnetInstance.invokeMethodAsync('UpdateTile', JSON.stringify(clickedTile));
}

export function initializeCanvas() {
    canvasWidth = tileSize * amountOfColumns;
    canvasHeight = tileSize * amountOfRows;

    canvas.width = canvasWidth;
    canvas.height = canvasHeight;
}

function clearCanvas() {
    context.clearRect(0, 0, canvasWidth, canvasHeight);
}

export function generateGrid() {
    
    if (tileSize === 0) return;

    initializeCanvas();
    clearCanvas();

    var tiles = [];

    for (var i = 0; i < amountOfRows; i++) {
        for (var j = 0; j < amountOfColumns; j++) {
            var tile = { X: j * tileSize, Y: i * tileSize, Checked:false, LineColor:"gray", FillColor:"transparent", LineWidth: "1px" }
            tiles.push(tile);
        }
    }

    dotnetInstance.invokeMethodAsync('SaveTiles', tiles);
}

export function drawLine(x1, y1, x2, y2, color = "black", lineWidth = "1px") {
    context.beginPath();
    context.moveTo(x1, y1);
    context.lineTo(x2, y2);
    context.lineWidth = lineWidth;
    context.strokeStyle = color;
    context.stroke();
}

export function drawRectangle(x, y, width, height, lineColor = "transparent", lineWidth = "1px", fillColor = "transparent") {
    context.beginPath();
    context.rect(x, y, width, height);
    context.lineWidth = lineWidth;
    context.strokeStyle = lineColor;
    context.stroke();
    context.fillStyle = fillColor;
    context.fill();
}

export function setTileSize(value) {
    tileSize = value;
}

export function setAmountOfColumns(value) {
    amountOfColumns = value;
}

export function setAmountOfRows(value) {
    amountOfRows = value;
}

export function redrawTile(tile) {
    clearRectangle(tile.x, tile.y, tileSize, tileSize);
    drawRectangle(tile.x, tile.y, tileSize, tileSize, tile.lineColor, tile.lineWidth, tile.fillColor);
}

function clearRectangle(x, y, width, height) {
    context.clearRect(x, y, width, height);
}


export function initializeDotNetInstance(instance) {
    dotnetInstance = instance;
}
