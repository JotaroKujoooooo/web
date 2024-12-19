function toggleElements(isLoggedIn) {
    document.getElementById('loginform').style.display = isLoggedIn ? "none" : "block";
    document.getElementById('alreadylogin').style.display = isLoggedIn ? "block" : "none";
    document.getElementById("admin").style.display = isLoggedIn && localStorage.getItem("nickname") === "admin" ? "block" : "none";
}
window.onload = async function() {
    let key = localStorage.getItem("accessToken");
    if (key!==null) {
        toggleElements(true)
    }
}
document.getElementById("logOut").addEventListener("click", e => {
    e.preventDefault();
    localStorage.removeItem("accessToken");
    localStorage.removeItem("nickname");
    toggleElements(false)
});

document.getElementById("enter").addEventListener("click", async e => {
    e.preventDefault();
    const response = await fetch("http://localhost:5096/User/login", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            login: document.getElementById("login").value,
            password: document.getElementById("password").value,
            email: "string"
        })
    });

    if (response.ok === true) {
        const data = await response.json();
        localStorage.setItem("accessToken", data.token);
        localStorage.setItem("nickname", data.username);
        toggleElements(true)
    }
});