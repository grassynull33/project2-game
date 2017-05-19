$(document).ready(function() {

    // links to our images in this image array.
    var images = ["assets/images/equili-logo-black-bg.png", "assets/images/equili-logo02.png", "assets/images/equili-logo-black-bg.png", "assets/images/equili-logo02.png"];

    // Variable showImage will hold the setInterval when we start the slideshow
    var showImage;

    // Count will keep track of the index of the currently displaying picture.
    var count = -1;

    // This function will replace display whatever image it's given
    // in the 'src' attribute of the img tag.
    function displayImage() {
        $("#brand-logo-holder").html("<img src=" + images[count] + " id='brand-logo' alt='Equilibrium'>");
    }

    function nextImage() {
        //  TODO: Increment the count by 1.

        count++;

        setTimeout(displayImage, 0);

        if (count === images.length - 1) {
            stopSlideshow();
        }
    }

    function startSlideshow() {
        // TODO: Use showImage to hold the setInterval to run nextImage.
        showImage = setInterval(nextImage, 2500);
    }

    function stopSlideshow() {
        clearInterval(showImage);
    }

    startSlideshow();
});
