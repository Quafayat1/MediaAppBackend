```js
   // Example: fetch and display media
   const apiBaseUrl = 'https://<your-api-name>.azurewebsites.net/api'; // Replace with your actual API URL

   // Event listeners
   document.getElementById('searchBtn').addEventListener('click', loadMedia);
   document.getElementById('loginBtn').addEventListener('click', login);
   document.getElementById('logoutBtn').addEventListener('click', logout);

   async function loadMedia() {
     const query = document.getElementById('searchInput').value || '';
     // For simplicity, let's say your API supports a query param like ?search=...
     const response = await fetch(`${apiBaseUrl}/media?search=${encodeURIComponent(query)}`);
     const mediaItems = await response.json();

     const mediaListDiv = document.getElementById('media-list');
     mediaListDiv.innerHTML = ''; // Clear old results

     mediaItems.forEach(item => {
       const div = document.createElement('div');
       div.innerHTML = `
         <h3>${item.title}</h3>
         <p>${item.caption}</p>
         <button onclick="viewMediaDetail(${item.mediaId})">View Details</button>
       `;
       mediaListDiv.appendChild(div);
     });
   }

   // Navigating to detail page
   function viewMediaDetail(mediaId) {
     // We can do a simple window location approach
     window.location.href = `media-detail.html?mediaId=${mediaId}`;
   }

   // Placeholder login/logout
   function login() {
     // This should redirect to your Azure AD B2C sign-in page or handle sign-in logic
     alert('Login flow not implemented in this basic example!');
   }
   function logout() {
     // Clear token, redirect as needed
   }

   // Load media on page load
   loadMedia();