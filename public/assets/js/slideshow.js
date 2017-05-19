$(document).ready(function() {
//  Slideshow Activity
//  ** SOLUTION **

// TODO: Put links to our images in this image array.
var images = ["assets/images/equili-logo-black-bg.png", "assets/images/equili-logo02.png", "assets/images/equili-logo-black-bg.png", "assets/images/equili-logo02.png"];

// Variable showImage will hold the setInterval when we start the slideshow
var showImage;

// Count will keep track of the index of the currently displaying picture.
var count = -1;

// // TODO: Use jQuery to run "startSlideshow" when we click the "start" button.
// $("#start").click(startSlideshow);

// // TODO: Use jQuery to run "stopSlideshow" when we click the "stop" button.
// $("#stop").click(stopSlideshow);


// This function will replace display whatever image it's given
// in the 'src' attribute of the img tag.
function displayImage() {
  $("#brand-logo-holder").html("<img src=" + images[count] + " id='brand-logo' alt='Equilibrium'>");
}

function nextImage() {
  //  TODO: Increment the count by 1.

    count++;
    console.log("count is" + count);
  // TODO: Show the loading gif in the "image-holder" div.
  // $("#brand-logo-holder").html("<img src='assets/images/avatar.jpg'/>");

  setTimeout(displayImage, 0);

  // TODO: If the count is the same as the length of the image array, reset the count to 0.
    if (count === images.length - 1) {
      stopSlideshow();
    }
    // TODO: Use a setTimeout to run displayImage after 1 second.

}

function startSlideshow() {
  // TODO: Use showImage to hold the setInterval to run nextImage.
  showImage = setInterval(nextImage, 2500);
}

function stopSlideshow() {

  // TODO: Put our clearInterval here:
  clearInterval(showImage);

}

// This will run the display image function as soon as the page loads.
startSlideshow();
});