const functions = require('firebase-functions');
const fetch = require('node-fetch');

exports.checkRecaptcha = functions.https.onRequest(async (req, res) => {
  const token = req.query.response;
  const secret = functions.config().recaptcha.secret;  // устанавливаем через CLI

  const resp = await fetch('https://www.google.com/recaptcha/api/siteverify', {
    method: 'POST',
    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    body: `secret=${secret}&response=${token}`
  });
  const json = await resp.json();

  if (json.success) res.send('OK');
  else res.status(400).send(json);
});