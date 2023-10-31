package br.com.itau.loyalty.application.validacao.validadores

import br.com.itau.loyalty.domain.Pedido
import br.com.itau.loyalty.domain.ProgramaConfigLista
import br.com.itau.loyalty.domain.enums.ErroProcessamentoEnum
import br.com.itau.loyalty.domain.exceptions.BusinessException
import io.mockk.every
import io.mockk.mockk
import io.mockk.spyk
import io.mockk.verify
import org.junit.jupiter.api.Assertions.assertEquals
import org.junit.jupiter.api.Assertions.assertThrows
import org.junit.jupiter.api.Test

class ValidaProdutoInformadoTest {

    @Test
    fun `test when parceiroConfig is null`() {
        val validator = ValidaProdutoInformado()
        val programaConfig = mockk<ProgramaConfigLista.ProgramaConfig>()
        val pedido = mockk<Pedido>()

        every { programaConfig.parceiros[any()] } returns null

        val exception = assertThrows(BusinessException::class.java) {
            validator.check(programaConfig, pedido)
        }

        assertEquals(ErroProcessamentoEnum.PRODUTO_NAO_INFORMADO, exception.error)
    }

    @Test
    fun `test when resgatePolaris is null`() {
        val validator = ValidaProdutoInformado()
        val programaConfig = mockk<ProgramaConfigLista.ProgramaConfig>()
        val pedido = mockk<Pedido>()
        val parceiroConfig = mockk<ProgramaConfigLista.ParceiroConfig>()

        every { programaConfig.parceiros[any()] } returns parceiroConfig
        every { parceiroConfig.resgatePolaris } returns null

        val exception = assertThrows(BusinessException::class.java) {
            validator.check(programaConfig, pedido)
        }

        assertEquals(ErroProcessamentoEnum.PRODUTO_NAO_INFORMADO, exception.error)
    }

    // ... (You'll continue with similar structure for the remaining scenarios)

    // For scenarios where we want to check if checkNext is called:
    @Test
    fun `test when resgateHabilitado is false, cancelamentoResgateHabilitado is true, and idProduto is not null`() {
        val validator = spyk(ValidaProdutoInformado())
        val programaConfig = mockk<ProgramaConfigLista.ProgramaConfig>()
        val pedido = mockk<Pedido>()
        val parceiroConfig = mockk<ProgramaConfigLista.ParceiroConfig>()
        val resgatePolaris = mockk<ProgramaConfigLista.ResgatePolaris>()

        every { programaConfig.parceiros[any()] } returns parceiroConfig
        every { parceiroConfig.resgatePolaris } returns resgatePolaris
        every { resgatePolaris.resgateHabilitado } returns false
        every { resgatePolaris.cancelamentoResgateHabilitado } returns true
        every { pedido.idProduto } returns "someId"

        validator.check(programaConfig, pedido)

        verify { validator.checkNext(programaConfig, pedido) }
    }

    // Similarly, you'll continue with the other scenarios...
}