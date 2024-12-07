    [SwaggerOperation(Summary = "Inserts commercial structure data",
        Description = " <b><i>A API foi desenvolvida para facilitar o cadastro " +
                      "da estrutura comercial de uma empresa, utilizando " +
                      "uma matriz de 8 posições. Cada posição representa " +
                      "um nível específico na hierarquia de gerenciamento, " +
                      "sendo a posição 1 correspondente à gerência de mais " +
                      "alto nível e a posição 8 à gerência " +
                      "de mais baixo nível.<i></b>")]
    [SwaggerResponse(StatusCodes.Status200OK, "Success inserts commercial structure data")]
    [SwaggerRequestExample(typeof(CommercialStructureRequest), typeof(CommercialStructureRequestFilter))]
