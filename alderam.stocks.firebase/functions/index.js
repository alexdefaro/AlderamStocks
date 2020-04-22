const functions = require('firebase-functions');
const admin = require('firebase-admin');

admin.initializeApp();

const Setores = require('./setores');
exports.getSetores = Setores.getSetores;
exports.addSetores = Setores.addSetores;

const Ativos = require('./ativos');
exports.getAtivos = Ativos.getAtivos;

