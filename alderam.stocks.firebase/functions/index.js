const functions = require('firebase-functions');
const admin = require('firebase-admin');

admin.initializeApp();

exports.helloWorld = functions.https.onRequest((request, response) => {
    response.send("Hello World.");
});


exports.getSetores = functions.https.onRequest((request, response) => {
    admin.firestore().collection('Setores').get()
        .then(data => {
            let setores = [];
            data.forEach(doc => {
                setores.push(doc.data());
            });
            return response.json(setores);
        })
        .catch(
            (error) => console.log(error)
        );
}); 