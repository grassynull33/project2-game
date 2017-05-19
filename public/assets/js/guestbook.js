$(document).ready(function() {
    // // Gets an optional query string from our url (i.e. ?post_id=23)
    // var url = window.location.search;

    // // If we have this section in our url, we pull out the post id from the url
    // // In localhost:8080/cms?post_id=1, postId is 1
    // if (url.indexOf("?post_id=") !== -1) {
    //   postId = url.split("=")[1];
    //   getPostData(postId);
    // }

    // Getting jQuery references to the post body, title, form, and category select
    var name = $("#name");
    var username = $("#username");
    var guestBookForm = $("#guestbook");
    var feedback = $("#feedback");
    // Adding an event listener for when the form is submitted
    $("#guestbookSubmit").click(function(event) {
        // event.preventDefault();
        // Wont submit the post if we are missing a body or a title
        // if (!name.val().trim() || !feedback.val().trim()) {
        //   return;
        // }
        // Constructing a newPost object to hand to the database
        var newFeedback = {
            name: name.val().trim(),
            feedback: feedback.val().trim(),
            username: username.val()
        };

        console.log(newFeedback);
        submitFeedback(newFeedback);


        // Submits a new post and brings user to blog page upon completion
        function submitFeedback(Feedback) {
            $.post("/api/feedback/", Feedback, function() {
                console.log(Feedback);
                // window.location.href = "/";
            });
        }
    })
});

// // Gets post data for a post if we're editing
// function getPostData(id) {
//   $.get("/api/feedback/" + id, function(data) {
//     if (data) {
//       // If this post exists, prefill our cms forms with its data
//       titleInput.val(data.title);
//       bodyInput.val(data.body);
//       postCategorySelect.val(data.category);
//       // If we have a post with this id, set a flag for us to know to update the post
//       // when we hit submit
//       updating = true;
//     }
//   });
// }

// Update a given post, bring user to the blog page when done
//   function updatePost(post) {
//     $.ajax({
//       method: "PUT",
//       url: "/api/posts",
//       data: post
//     })
//     .done(function() {
//       window.location.href = "/blog";
//     });
//   }
// });
