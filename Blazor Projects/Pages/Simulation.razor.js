"use strict";

var canvas = document.getElementById("tile-canvas");
var context = canvas.getContext("2d");

var tileSize;
var canvasWidth;
var canvasHeight;

var amountOfRows;
var amountOfColumns;
var squaresTopMostLeftCoordinates = []
var dotnetInstance;

registerWindowResizeHandler();
registerClickHandler();
setCanvasSize();


function registerWindowResizeHandler() {
    addEventListener("resize", (e) => { handleWindowResize() });
}

function registerClickHandler() {
    canvas.addEventListener("click", (e) => { handleOnClick(e) });
}

function handleWindowResize() {
    setCanvasSize();
    drawGrid();
}

function handleOnClick(e) {
    var tile = canvas.getBoundingClientRect();
    var tileX = e.clientX - tile.left;
    var tileY = e.clientY - tile.top;

    var clickedRow = Math.floor(tileY / tileSize);
    var clickedCol = Math.floor(tileX / tileSize);

    console.log()

    redrawTile(clickedCol, clickedRow);
}

function setCanvasSize() {
    canvasWidth = tileSize * amountOfColumns;
    canvas.width = canvasWidth;
    canvasHeight = tileSize * amountOfRows;
    canvas.height = canvasHeight;
}

function clearCanvas() {
    context.clearRect(0, 0, canvasWidth, canvasHeight);
}

function drawGrid() {
    if (tileSize === 0) return;

    setCanvasSize();
    clearCanvas();

    for (var i = 0; i <= amountOfRows; i++) {
        for (var j = 0; j <= amountOfColumns; j++) {
            drawRectangle(j * tileSize, i * tileSize, tileSize, tileSize, "transparent");
            var topMostLeftCoordinate = { x: j * tileSize, y: i * tileSize }
            squaresTopMostLeftCoordinates.push(topMostLeftCoordinate);
        }
    }

    dotnetInstance.invokeMethodAsync('GetTileCoordinates', squaresTopMostLeftCoordinates);
}

function drawLine(x1, y1, x2, y2, color = "black", lineWidth = "1px") {
    context.beginPath();
    context.moveTo(x1, y1);
    context.lineTo(x2, y2);
    context.lineWidth = lineWidth;
    context.strokeStyle = color;
    context.stroke();
}

function drawRectangle(x, y, width, height, color = "black", lineWidth = "1px") {
    context.beginPath();
    context.rect(x, y, width, height);
    context.lineWidth = lineWidth;
    context.strokeStyle = color;
    context.stroke();
}

export function setTileSize(value) {
    tileSize = value;
    drawGrid();
}

export function setAmountOfColumns(value) {
    amountOfColumns = value;
    drawGrid();
}

export function setAmountOfRows(value) {
    amountOfRows = value;
    drawGrid();
}

function redrawTile(x, y) {
    var topLeftCoordinateX = x * tileSize;
    var topLeftCoordinateY = y * tileSize;

    context.clearRect(topLeftCoordinateX, topLeftCoordinateY, tileSize, tileSize);
    context.fillStyle = "hotpink"
    context.fillRect(topLeftCoordinateX, topLeftCoordinateY, tileSize, tileSize); 

    var tileCoordinates = { X: topLeftCoordinateX, Y: topLeftCoordinateY };
    dotnetInstance.invokeMethodAsync('UpdateTileCheckedState', tileCoordinates );
}

export function getTileCoordinates(dotNetHelper) {
    dotnetInstance = dotNetHelper;
}
