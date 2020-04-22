const functions = require('firebase-functions');
const admin = require('firebase-admin');

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

exports.addSetores = functions.https.onRequest((request, response) => {
    const newDocument = {
        Codigo: request.body.codigo,
        Nome: request.body.nome
    }
    admin.firestore().collection('Setores').add(newDocument)
        .then(function (data) {
            response.json("Document written with ID: " + data.id);
        })
        .catch(function (error) {
            response.status(500).json({ error })
        });
}); 