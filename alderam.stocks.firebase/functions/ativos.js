const functions = require('firebase-functions');
const admin = require('firebase-admin');
 
exports.getAtivos = functions.https.onRequest((request, response) => {
    admin.firestore().collection('Ativos').get()
        .then(data => {
            let registros = [];
            data.forEach(doc => {
                registros.push(doc.data());
            });
            return response.json(registros);
        })
        .catch(
            (error) => console.log(error)
        );
}); 