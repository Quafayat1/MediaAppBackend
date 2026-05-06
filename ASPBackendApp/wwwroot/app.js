const baseUrl = "https://your-app-name.azurewebsites.net/api/posts";

// GET all posts
async function getPosts() {
    const res = await fetch(baseUrl);
    const data = await res.json();
    const postsList = document.getElementById("postsList");
    postsList.innerHTML = "";
    data.result.forEach(post => {
        const li = document.createElement("li");
        li.textContent = `${post.title} by ${post.author}`;
        postsList.appendChild(li);
    });
}

// POST a new post
async function addPost() {
    const title = document.getElementById("title").value;
    const content = document.getElementById("content").value;
    const author = document.getElementById("author").value;

    const res = await fetch(baseUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, content, author })
    });

    const data = await res.json();
    alert(data.isSuccess ? "Post added!" : "Error: " + data.message);
    getPosts(); // refresh posts
}
