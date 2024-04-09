import express from 'express';
import fs from 'fs'
const app = express();
app.use(express.json());
app.use(express.static('nehsanet/dist/nehsanet/browser'))
const port = process.env.PORT || 80;


app.listen(port, () => {
    console.log(`Listing on port ${port}`);
});


/**
 * @function /api/v1/contactme
 * @param {{body: string}} req - The body.subject and body.body of the request is JSON format.  e.g. { body.subject = 'test subject', body.body = 'test body' }
 * @return {boolean} res - The result from the api
 */
app.post('/api/v1/contactme', (req, res) => {
    let content = `${req.body.subject}: ${req.body.body}\n`
    let success = true;
    fs.writeFile('contactme.txt', content, { flag: 'a+' }, err => {
        if (err) {
            success = false;
        } else {
            console.log(`A contactme submission was captured:\n${content}`);
        }
      });
      res.send({ data: success });
});
