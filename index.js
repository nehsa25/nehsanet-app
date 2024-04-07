import express from 'express';
const app = express();
app.use(express.json());
app.use(express.static('nehsanet/dist/nehsanet/browser'))
const port = process.env.PORT || 80;
app.listen(port, () => {
    console.log(`Listing on port ${port}`);
});

app.get('/api/test/:id', (req, res) => {
    const id = req.params.id;
    if (id === 1) {
        res.status(404).send({ error: 'example 404' });
    } else {
        res.send({ data: "yay" });
    }
});

