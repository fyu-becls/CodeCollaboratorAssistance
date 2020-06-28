window.blazorTextDiff = {
    getHeight: function (element) {
        return element.offsetHeight;
    },
    getDimensions: function () {
        return {
            width: window.innerWidth,
            height: window.innerHeight
        };
    },
    scrollIntoView: function (element) {
        document.getElementById(element).scrollIntoView();
    }
};