function interpolateColors(steps) {
    var data = new Array(steps);
    var frequency = 2 * Math.PI / steps;
    var redFrequency = frequency; //1.666;
    var grnFrequency = frequency; //2.666;
    var bluFrequency = frequency; //3.666;
    var phase1 = 0; //0
    var phase2 = 2; //2
    var phase3 = 4; //4
    var center = 128;
    var width = 127;
    for (var i = 0; i < steps; ++i) {
        var r = Math.sin(redFrequency * i + phase1) * width + center;
        var g = Math.sin(grnFrequency * i + phase2) * width + center;
        var b = Math.sin(bluFrequency * i + phase3) * width + center;
        r /= 255.0, g /= 255.0, b /= 255.0;
        var max = Math.max(r, g, b), min = Math.min(r, g, b);
        var h, s, l = (max + min) / 2.0;
        if (max == min) {
            h = s = 0.0;
        } else {
            var d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            switch (max) {
                case r: h = (g - b) / d + (g < b ? 6.0 : 0.0); break;
                case g: h = (b - r) / d + 2; break;
                case b: h = (r - g) / d + 4; break;
            }
            h /= 6.0;
        }
        data[i] = 'hsl(' + ((h * 100 + 0.5) | 0) + ',' + ((s * 100 + 0.5) | 0) + '%,' + ((l * 100 + 0.5) | 0) + '%)';
    }
    return data;
}