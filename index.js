import express from 'express';
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
    const request = req.json();
    const subject = request.contact_subject;
    const body = request.contact_body;
    if (id === 1) {
        res.status(404).send({ error: 'example 404' });
    } else {
        res.send({ data: `You sent body: ${body}` });
    }
});



