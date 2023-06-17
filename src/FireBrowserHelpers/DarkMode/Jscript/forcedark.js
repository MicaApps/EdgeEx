// Based on code from https://zsmith.co/dark.php
var body = document.getElementsByTagName("BODY")[0];
var html = document.getElementsByTagName("HTML")[0];
html.style.backgroundColor = "#202020";
body.style.backgroundColor = "#202020";
body.style.color = "#f0e000";
var tags = ["FOOTER", "HEADER", "MAIN", "SECTION",
    "NAV", "FORM",
    "FONT", "EM", "B", "I", "U",
    "INPUT", "P", "BUTTON", "OL", "UL", "A", "DIV",
    "TD", "TH", "SPAN", "LI",
    "H1", "H2", "H3", "H4", "H5", "H6",
    "DD", "DT",
    "INCLUDE-FRAGMENT", "ARTICLE"
];
for (let tag of tags) {
    for (let item of document.getElementsByTagName(tag)) {
        item.style.backgroundColor = "#202020";
        item.style.color = "white";
    }
}
for (let tag of ["CODE", "PRE"]) {
    for (let item of document.getElementsByTagName(tag)) {
        item.style.backgroundColor = "#202020";
        item.style.color = "green";
    }
}
for (let tag of document.getElementsByTagName("INPUT")) {
    tag.style.border = "solid 1px #bbb";
}
var videos = document.getElementsByTagName("VIDEO");
for (let video of videos) {
    video.style.backgroundColor = "#202020";
}
for (let tag of document.getElementsByTagName("TH")) {
    tag.style.borderBottom = "solid 1px yellow";
}
for (let tag of document.getElementsByTagName("A")) {
    tag.style.color = "cyan";
}