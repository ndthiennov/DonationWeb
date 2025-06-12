importScripts('https://www.gstatic.com/firebasejs/8.10.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.10.1/firebase-messaging.js');

firebase.initializeApp({
    apiKey: "AIzaSyAJoExvhrKFzKpw6j67s2qPgsIQfFuK6cE",
    authDomain: "donationpushnotification.firebaseapp.com",
    projectId: "donationpushnotification",
    // storageBucket: "donationpushnotification.appspot.com",
    storageBucket: "donationpushnotification.firebasestorage.app",
    messagingSenderId: "662459039023",
    appId: "1:662459039023:web:56da5e09fc3d15a29af5da"
});

const messaging = firebase.messaging();

messaging.onBackgroundMessage((payload) => {
    // console.log("hello: ", payload)
})

self.addEventListener("push", function (event) {
    const payload = event.data.json();

    self.clients.matchAll({ type: "window", includeUncontrolled: true }).then(function (clients) {
        console.log(clients);
        if (clients.length > 0) {
            clients.forEach(client => {
                // client.postMessage({
                //     type: "Background_Message",
                //     payload: payload,
                // })

                client.postMessage(payload);
            });
        }
        else {
            self.clients.openWindow("/").then(client => {
                client.postMessage(payload);
            })
        }
    })
})