import express from 'express';
import fs from 'fs'
const app = express();
app.use(express.json());
app.use(express.static('nehsanet/dist/nehsanet/browser'))
const port = process.env.PORT || 80;
app.listen(port, () => {
    console.log(`Listing on port ${port}`);
});

app.get('/api/v1/contactme', (req, res) => {
    const id = req.params.id;
    if (id === 1) {
        res.status(404).send({ error: 'example 404' });
    } else {
        res.send({ data: "yay" });
    }
});


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
