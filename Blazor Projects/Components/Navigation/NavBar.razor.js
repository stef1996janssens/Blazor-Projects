"use strict";

export function getNavBarWidth() {
    var navBar = document.getElementById("main-nav");
    return `${navBar.offsetWidth}px`;
}

export function getNavBarHeight() {
    var navBar = document.getElementById("main-nav");
    return `${navBar.offsetHeight}px`;
}