$('#mapLoad').click(function () {
  var scene, camera, renderer;

  init();
  animate();

    // Sets up the scene.
  function init () {
      // Create the scene and set the scene size.
    scene = new THREE.Scene();
    var WIDTH = window.innerWidth,
      HEIGHT = window.innerHeight;

      // Create a renderer and add it to the DOM.
    renderer = new THREE.WebGLRenderer();
    renderer.setSize(WIDTH, HEIGHT);
    $('#myCanvas').append(renderer.domElement);
      // container.appendChild(renderer.domElement);

      // Create a camera, zoom it out from the model a bit, and add it to the scene.
    camera = new THREE.PerspectiveCamera(45, WIDTH / HEIGHT, 0.1, 20000);
    camera.position.set(486, 466.60, -55.60);
    scene.add(camera);

      // Create an event listener that resizes the renderer with the browser window.
    window.addEventListener('resize', function () {
      var WIDTH = window.innerWidth,
        HEIGHT = window.innerHeight;
      renderer.setSize(WIDTH, HEIGHT);
      camera.aspect = WIDTH / HEIGHT;
      camera.updateProjectionMatrix();
    });

      // Load in the mesh and add it to the scene.
    var loader = new THREE.ObjectLoader();
    loader.load('assets/js/model2.json', function (object) {
      scene.add(object);
    });

      // Add OrbitControls so that we can pan around with the mouse.
    controls = new THREE.OrbitControls(camera, renderer.domElement);
  }

    // Renders the scene and updates the render as needed.
  function animate () {
      // Read more about requestAnimationFrame at http://www.paulirish.com/2011/requestanimationframe-for-smart-animating/
    requestAnimationFrame(animate);

      // Render the scene.
    renderer.render(scene, camera);
    controls.update();
  }
});

$('#mapUnload').click(function () {
  window.location.reload(); // Should send you back to Map before you opened Modal to continue your journey :)
});
